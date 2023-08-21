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

    [Header("プレイヤーのゲームオブジェクト")]
    private GameObject player;

    [Header("エネミータグの名前")]
    [SerializeField] string enemyTag = "Enemy";


    public void SetValues(GameObject enemy, float bulletSpd, GameObject player)
    {
        this.enemy = enemy;
        this.bulletSpd = bulletSpd;
        this.player = player;
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
        else Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != player && collision.gameObject != gameObject)
        {
            if (collision.gameObject.tag == enemyTag)
            {
                player.GetComponent<MouseLockOnShooting>().DestroyEnemy(collision.gameObject);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
