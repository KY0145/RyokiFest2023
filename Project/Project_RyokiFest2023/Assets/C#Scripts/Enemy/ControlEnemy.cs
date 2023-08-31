using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemy : MonoBehaviour
{
    private GameObject player;

    [Header("プレイヤーの名前")]
    [SerializeField] private string playerName;

    [SerializeField] private EnemyParam enemyParam;

    /// <summary>
    /// デバッグ用にインスペクターで表示
    /// </summary>
    public float HP;


    void Start()
    {
        HP = enemyParam.HP;
        player = GameObject.Find(playerName);
    }

    void Update()
    {
        if (HP <= 0)
        {
            player.GetComponent<MouseLockOnShooting>().enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
