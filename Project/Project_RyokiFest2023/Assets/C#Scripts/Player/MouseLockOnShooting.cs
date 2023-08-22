using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �e�ۂ𔭎˂���Ƃ��ɁA�X�N���[����œG�ɃJ�[�\�������킹�Ă���Ǝ����ł��̈ʒu�Ɍ����Ĕ��˂���悤�ɂ��邽�߂̃X�N���v�g
/// </summary>


public class MouseLockOnShooting : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private Camera targetCamera;

    [Header("�e�ۂ𐶐�����Ƃ��Ƀv���C���[�����_�Ƃ��Ăǂ̈ʒu�ɐ������邩���w�肷��x�N�g��3")]
    [SerializeField] private Vector3 bulletCreatePos_playerLocal;

    [Header("���˂���e�ۂ�Prefab")]
    [SerializeField] private GameObject bullet;

    [Header("�e�ۂ̃X�s�[�h")]
    [SerializeField] private float bulletSpd;

    /// <summary>
    /// �G�ɂ���Ă��̒l�͕ς���K�v�����遨�G��̂��ƂɌŗL�Ɋ���U��K�v����
    /// </summary>
    [Header("�G�̍��W�ƃ}�E�X���W�̋��e�덷")]
    [SerializeField] private float distError;

    [Header("���b�N�I������G�̃��X�g")]
    public List<GameObject> enemies;

    /// <summary>
    /// ���b�N�I������G�̉��̃��X�g
    /// </summary>
    [SerializeField] private List<GameObject> targetEnemiesList;



    void Update()
    {
        targetEnemiesList = new List<GameObject>();

        Vector2 mousePos = Input.mousePosition; //�}�E�X�J�[�\���̃X�N���[�����W
        
        foreach (var enemy in enemies)
        {
            //�G�̃��[���h���W���X�N���[�����W�ɕϊ�
            Vector2 enemyPos = targetCamera.WorldToScreenPoint(enemy.transform.position);

            if (Input.GetKeyDown(KeyCode.Mouse0) && IsRockON(mousePos, enemyPos))
            {
                targetEnemiesList.Add(enemy);
            }
        }


        if (targetEnemiesList.Count != 0)
        {
            //�X�N���[�����W�ŕ����̃I�u�W�F�N�g���d�Ȃ����ꍇ�A�ł�z���W��������(~�ł���O�ɕ`�悳��Ă���)�I�u�W�F�N�g��I��
            //���P�̗]�n����(z���W��P���ɔ�r���邾���ł͑Ή��ł��Ȃ��ꍇ�����㔭������\�������邽��)
            int index = 0;
            float dis = 0;
            for (int i = 0; i < targetEnemiesList.Count; i++)
            {
                if (i == 0)
                {
                    dis = targetEnemiesList[i].transform.position.z;
                    index = i;
                }
                else
                {
                    if (dis > targetEnemiesList[i].transform.position.z)
                    {
                        dis = targetEnemiesList[i].transform.position.z;
                        index = i;
                    }
                }
            }

            //����
            Fire(player, targetEnemiesList[index], bulletCreatePos_playerLocal, bulletSpd);
        }
    }


    /// <summary>
    /// �}�E�X�J�[�\����G�ɍ��킹�Ă��邩����
    /// </summary>
    /// <param name="mousePos">�}�E�X�J�[�\���̃X�N���[�����W</param>
    /// <param name="enemyPos">�G�̃X�N���[�����W</param>
    /// <returns></returns>
    bool IsRockON(Vector2 mousePos, Vector2 enemyPos)
    {
        Debug.Log((mousePos - enemyPos).magnitude);
        return (mousePos - enemyPos).magnitude <= distError;
    }


    /// <summary>
    /// �e�ۂ�G�̈ʒu�ɔ��˂���
    /// </summary>
    /// <param name="player">�v���C���[�̃Q�[���I�u�W�F�N�g</param>
    /// <param name="enemy">�G�̃Q�[���I�u�W�F�N�g</param>
    /// <param name="createPos">�e�ۂ��쐬����ʒu���W�i�v���C���[�����_�Ƃ������[�J�����W�j</param>
    void Fire(GameObject player, GameObject enemy, Vector3 createPos, float bulletSpd)
    {
        //�e�ۂ𐶐�
        GameObject bl = Instantiate(bullet);
        
        //���W���C��
        bl.transform.position = player.transform.position + createPos;

        bl.GetComponent<ControlBullet>().SetValues(enemy, bulletSpd, gameObject);
    }


    /// <summary>
    /// enemies�̗v�f���폜
    /// </summary>
    /// <param name="enemy">�폜����v�f</param>
    public void DestroyEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
