using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾丸の削除・速度ベクトルの制御を行う
/// </summary>

public class ControlBullet : MonoBehaviour
{
    /// <summary>
    /// 敵のゲームオブジェクト
    /// </summary>
    private GameObject enemy;

    /// <summary>
    /// 弾丸の速度
    /// </summary>
    private float bulletSpd;

    /// <summary>
    /// 攻撃力
    /// </summary>
    private float power;

    [Header("弾丸が目標を追尾する")]
    [SerializeField] private bool isTracing;

    private int tracingFrames;
    private int nowFrame = 0;

    [Header("目標エネミーが消えた時、一緒に弾丸も消える")]
    [SerializeField] private bool isDestroyWhenEnemyIsDestroied;

    [Header("プレイヤーからどのくらい離れたら消えるか")]
    [SerializeField] private float destroyDis;

    /// <summary>
    /// プレイヤーのゲームオブジェクト
    /// </summary>
    private GameObject player;



    public void SetValues(GameObject enemy, float bulletSpd, GameObject player, float power, bool isTracing, int tracingFrames)
    {
        this.enemy = enemy;
        this.bulletSpd = bulletSpd;
        this.player = player;
        this.power = power;
        this.isTracing = isTracing;
        this.tracingFrames = tracingFrames;
    }

    void FixedUpdate()
    {
        if ((player.transform.position - gameObject.transform.position).magnitude > destroyDis)
        {
            Destroy(gameObject);
        }

        if (enemy != null && isTracing && nowFrame < tracingFrames)
        {
            //方向ベクトル
            Vector3 blToEnemy = enemy.transform.position - transform.position;

            //敵の方角へ進む
            GetComponent<Rigidbody>().velocity = blToEnemy / blToEnemy.magnitude * bulletSpd;
        }
        else if (enemy == null)
        {
            if(isDestroyWhenEnemyIsDestroied)
            {
                Destroy(gameObject);
            }
        }

        nowFrame++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != player && !collision.gameObject.name.Contains(gameObject.name))
        {
            try
            {
                var enemy = collision.gameObject;
                if (player.name.Contains("Enemy"))
                {
                    var HP = enemy.GetComponent<ControlEnemy>().HP;
                    HP -= power;
                    if (HP <= 0)
                    {
                        enemy.GetComponent<ControlEnemy>().DestroyMe(0);
                    }
                    else
                    {
                        enemy.GetComponent<ControlEnemy>().HP = HP;
                    }
                }
                else
                {
                    enemy.GetComponent<ControlEnemy>().HP -= power;
                }
            }
            catch(NullReferenceException)
            {
                collision.gameObject.GetComponent<ControlPlayer>().HP -= power;
            }
            finally
            {
                Destroy(gameObject);
            }
        }
    }
}
