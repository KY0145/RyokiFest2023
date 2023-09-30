using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemy : MonoBehaviour
{
    [Header("プレイヤー")]
    public GameObject player;

    private Rigidbody rb;

    [SerializeField] private EnemyParam enemyParam;

    [Header("見えるようになる距離")]
    [SerializeField] private float spawnDis;

    [Header("行動を始める距離")]
    [SerializeField] private float doDis;

    [Header("加える力の大きさ")]
    [SerializeField] private float force;

    [Header("次の地点に進み始める距離")]
    [SerializeField] private float rotateDis;

    [Header("制限速度")]
    [SerializeField] private float limitVelo;

    [Header("攻撃する回数")]
    [SerializeField] private int roopCount;
    
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
    /// <param name="createPos">弾丸を作成する位置座標（プレイヤーを原点としたローカル座標）</param>
    /// <param name="bulletSpd">弾丸の速度</param>
    void Fire(GameObject player, GameObject enemy)
    {
        //弾丸を生成
        GameObject bl = Instantiate(enemyParam.bullet);

        //座標を修正
        bl.transform.position = player.transform.position + enemyParam.createPos;

        bl.GetComponent<ControlBullet>().SetValues(enemy, enemyParam.bulletSpd, player, enemyParam.power, enemyParam.isTracing, enemyParam.tracingFrames);

        Vector3 blToEnemy = enemy.transform.position - player.transform.position;

        bl.GetComponent<Rigidbody>().velocity = blToEnemy / blToEnemy.magnitude * enemyParam.bulletSpd;
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
