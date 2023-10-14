using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemy : MonoBehaviour
{
    [Header("プレイヤー")]
    public GameObject player;

    public GameObject stdPosPlayer;

    private Rigidbody rb;

    public EnemyParam enemyParam;

    public GameObject scoreManager;


    private float power = 1f;

    private float bodyPower = 0f;

    private Vector3 createPos;

    private float bulletSpd;

    private bool isTracing;

    private int tracingFrames;

    private GameObject bullet;


    private float spawnDis;

    private float doDis;

    private float force;

    private float rotateDis;

     private float limitVelo;

    private int roopCount;
    private float roopSeconds;

    private float score;

    [Header("移動する地点")]
    [SerializeField] private Vector3[] points;


    /// <summary>
    /// デバッグ用にインスペクターで表示
    /// </summary>
    public float HP;

    /// <summary>
    /// pointsのインデックス用変数
    /// </summary>
    private int I = 0;


    void Start()
    {
        HP = enemyParam.HP;
        power = enemyParam.power;
        bodyPower = enemyParam.bodyPower;
        createPos = enemyParam.createPos;
        bulletSpd = enemyParam.bulletSpd;
        isTracing = enemyParam.isTracing;
        tracingFrames = enemyParam.tracingFrames;
        bullet = enemyParam.bullet;

        spawnDis = enemyParam.spawnDis;
        doDis = enemyParam.doDis;
        force = enemyParam.force;
        rotateDis = enemyParam.rotateDis;
        limitVelo = enemyParam.limitVelo;
        roopCount = enemyParam.roopCount;
        roopSeconds = enemyParam.roopSeconds;
        score = enemyParam.score;

        rb = GetComponent<Rigidbody>();

        //攻撃コルーチンを開始
        StartCoroutine(FireCoroutine(roopCount, roopSeconds));

        //グローバル座標をプレイヤーを中心としたローカルに変換
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = CalcVector.Rotate(points[i], -stdPosPlayer.transform.eulerAngles.y * Mathf.PI / 180);
            points[i] += stdPosPlayer.transform.position;
        }
    }

    void FixedUpdate()
    {
        //設定距離以上のときアクティブ化
        if ((stdPosPlayer.transform.position - transform.position).magnitude <= spawnDis)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

        //設定距離以上のとき行動する
        if ((stdPosPlayer.transform.position - transform.position).magnitude <= doDis)
        {
            //移動
            Move();
        }

        if (HP <= 0)
        {
            DestroyMe(score);
        }
    }


    //プレイヤーと衝突したときにダメージを与える
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            collision.gameObject.GetComponent<ControlPlayer>().HP -= bodyPower;
            DestroyMe(0);
        }
    }


    public void DestroyMe(float score)
    {
        scoreManager.GetComponent<ScoreManager>().AddScore(score);
        player.GetComponent<MouseLockOnShooting>().enemies.Remove(gameObject);
        Destroy(gameObject);
    }


    void Move()
    {
        Vector3 direction = points[I] - transform.position;
        direction /= direction.magnitude;

        rb.AddForce(direction * force);

        //向きを修正
        transform.eulerAngles = Vector3.zero;
        transform.eulerAngles = ReturnEulerDeg(direction, transform.eulerAngles);

        //制限速度を超えた時は修正
        if (rb.velocity.magnitude > limitVelo)
        {
            rb.velocity /= (rb.velocity.magnitude / limitVelo);
        }

        if ((transform.position - points[I]).magnitude <= rotateDis)
        {
            I++;
        }

        if (I >= points.Length)
        {
            DestroyMe(0);
        }
    }

    /// <summary>
    /// 弾丸を敵の位置に発射する
    /// </summary>
    /// <param name="player">プレイヤーのゲームオブジェクト</param>
    /// <param name="enemy">敵のゲームオブジェクト</param>
    void Fire(GameObject player, GameObject enemy)
    {
        //弾丸を生成
        GameObject bl = Instantiate(bullet);

        //座標を修正
        bl.transform.position = player.transform.position + createPos;

        bl.GetComponent<ControlBullet>().SetValues(enemy, bulletSpd, player, power, isTracing, tracingFrames);

        Vector3 blToEnemy = enemy.transform.position - player.transform.position;

        bl.GetComponent<Rigidbody>().velocity = blToEnemy / blToEnemy.magnitude * bulletSpd;
    }

    /// <summary>
    /// 攻撃をループ
    /// </summary>
    /// <param name="counts">攻撃回数</param>
    /// <param name="seconds">攻撃間隔</param>
    IEnumerator FireCoroutine(int counts, float seconds)
    {
        for (int i = 0; i < counts; i++)
        {
            Fire(gameObject, player);
            yield return new WaitForSeconds(seconds);
        }
    }


    /// <summary>
    /// プレイヤーからベクトルbへのオイラー角度を求める
    /// </summary>
    /// <param name="b">目標ベクトル</param>
    /// <param name="eulerAngles">元のオイラー角</param>
    /// <returns>オイラー角度をVector3で返す</returns>
    Vector3 ReturnEulerDeg(Vector3 b, Vector3 eulerAngles)
    {
        var a = CalcVector.ReturnDirection(eulerAngles.y, 1, Vector3.zero);

        var a_xz = new Vector2(a.x, a.z);
        var b_xz = new Vector2(b.x, b.z);
        var yDeg = -CalcVector.CalcVecRad(a_xz, b_xz, true) * 180 / Mathf.PI;

        a = CalcVector.ReturnDirection(eulerAngles.y + yDeg, 1, Vector3.zero);

        var a_zy = new Vector2(a.z, a.y);
        var b_zy = new Vector2(b.z, b.y);
        var xDeg = CalcVector.CalcVecRad(a_zy, b_zy, true) * 180 / Mathf.PI;


        return new Vector3(eulerAngles.x + xDeg, eulerAngles.y + yDeg, eulerAngles.z);
    }
}
