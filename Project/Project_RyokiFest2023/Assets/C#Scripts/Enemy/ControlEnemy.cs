using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemy : MonoBehaviour
{
    [Header("プレイヤー")]
    public GameObject player;

    private Rigidbody rb;

    [SerializeField] private EnemyParam enemyParam;


    private float power = 1f;

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

        rb = GetComponent<Rigidbody>();

        //攻撃コルーチンを開始
        StartCoroutine(FireCoroutine(roopCount));

        //デバッグ用に追加
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = CalcVector.Rotate(points[i], -player.transform.eulerAngles.y * Mathf.PI / 180);
            points[i] += player.transform.position;
        }
    }

    void FixedUpdate()
    {
        //設定距離以上のときアクティブ化
        if ((player.transform.position - transform.position).magnitude <= spawnDis)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

        //設定距離以上のとき行動する
        if ((player.transform.position - transform.position).magnitude <= doDis)
        {
            //移動
            Move();
        }

        if (HP <= 0)
        {
            DestroyMe();
        }
    }


    private void DestroyMe()
    {
        player.GetComponent<MouseLockOnShooting>().enemies.Remove(gameObject);
        Destroy(gameObject);
    }


    void Move()
    {
        Vector3 direction = points[I] - transform.position;
        direction /= direction.magnitude;

        rb.AddForce(direction * force);

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
            DestroyMe();
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

    IEnumerator FireCoroutine(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            Fire(gameObject, player);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
