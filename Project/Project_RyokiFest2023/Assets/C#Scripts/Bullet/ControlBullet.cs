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

    [Header("目標エネミーが消えた時、一緒に弾丸も消える")]
    [SerializeField] private bool isDestroyWhenEnemyIsDestroied = true;

    /// <summary>
    /// プレイヤーのゲームオブジェクト
    /// </summary>
    private GameObject player;

    [Header("エネミータグの名前")]
    [SerializeField] string enemyTag = "Enemy";


    public void SetValues(GameObject enemy, float bulletSpd, GameObject player, float power)
    {
        this.enemy = enemy;
        this.bulletSpd = bulletSpd;
        this.player = player;
        this.power = power;
    }

    void Update()
    {
        if (gameObject.transform.position.magnitude > 100)
        {
            Destroy(gameObject);
        }

        if (enemy != null)
        {
            //方向ベクトル
            Vector3 blToEnemy = enemy.transform.position - transform.position;

            //敵の方角へ進む
            GetComponent<Rigidbody>().velocity = blToEnemy / blToEnemy.magnitude * bulletSpd;
        }
        else
        {
            if(isDestroyWhenEnemyIsDestroied)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject != player && !collision.gameObject.name.Contains(gameObject.name))
        {
            if (collision.gameObject.tag == enemyTag)
            {
                WhenCollisionEnemy(collision);
            }
            else
            {
                WhenCollisionAny();
            }
        }
    }


    void WhenCollisionEnemy(Collision collision)
    {
        /*
        //player.GetComponent<MouseLockOnShooting>().DestroyEnemy(collision.gameObject);
        player.GetComponent<MouseLockOnShooting>().enemies.Remove(collision.gameObject);
        Destroy(collision.gameObject);
        Destroy(gameObject);*/

        collision.gameObject.GetComponent<ControlEnemy>().HP -= power;
        Destroy(gameObject);
    }


    void WhenCollisionAny()
    {
        Destroy(gameObject);
    }
}
