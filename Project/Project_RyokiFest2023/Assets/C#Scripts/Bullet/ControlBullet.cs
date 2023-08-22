using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�ۂ̍폜�E���x�x�N�g���̐�����s��
/// </summary>

public class ControlBullet : MonoBehaviour
{
    /// <summary>
    /// �G�̃Q�[���I�u�W�F�N�g
    /// </summary>
    private GameObject enemy;

    /// <summary>
    /// �e�ۂ̑��x
    /// </summary>
    private float bulletSpd;

    [Header("�v���C���[�̃Q�[���I�u�W�F�N�g")]
    private GameObject player;

    [Header("�G�l�~�[�^�O�̖��O")]
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
            //�����x�N�g��
            Vector3 blToEnemy = enemy.transform.position - transform.position;

            //�G�̕��p�֐i��
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
