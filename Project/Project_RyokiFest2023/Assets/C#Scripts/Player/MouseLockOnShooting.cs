using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 弾丸を発射するときに、スクリーン上で敵にカーソルを合わせていると自動でその位置に向けて発射するようにするためのスクリプト
/// </summary>


public class MouseLockOnShooting : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private Camera targetCamera;

    [Header("左クリックをしなくても自動で毎フレーム弾丸を発射する")]
    [SerializeField] private bool isRensya = false;

    [Header("弾丸を生成するときにプレイヤーを原点としてどの位置に生成するかを指定するベクトル3")]
    [SerializeField] private Vector3 bulletCreatePos_playerLocal;

    [Header("発射する弾丸のPrefab")]
    [SerializeField] private GameObject bullet;

    [Header("弾丸のスピード")]
    [SerializeField] private float bulletSpd;

    [Header("攻撃力")]
    [SerializeField] private float power;

    /// <summary>
    /// 敵によってこの値は変える必要がある→敵一体ごとに固有に割り振る必要あり
    /// </summary>
    [Header("敵の座標とマウス座標の許容誤差")]
    [SerializeField] private float distError;

    [Header("ロックオンする敵のリスト")]
    public List<GameObject> enemies;

    /// <summary>
    /// ロックオンする敵の仮のリスト
    /// </summary>
    [SerializeField] private List<GameObject> targetEnemiesList;



    void Update()
    {
        targetEnemiesList = new List<GameObject>();

        Vector2 mousePos = Input.mousePosition; //マウスカーソルのスクリーン座標

        foreach (var enemy in enemies)
        {
            //敵のワールド座標をスクリーン座標に変換
            Vector2 enemyPos = targetCamera.WorldToScreenPoint(enemy.transform.position);

            if (isRensya)
            {
                if (IsRockON(mousePos, enemyPos))
                {
                    targetEnemiesList.Add(enemy);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && IsRockON(mousePos, enemyPos))
                {
                    targetEnemiesList.Add(enemy);
                }
            }
        }


        if (targetEnemiesList.Count != 0)
        {
            //スクリーン座標で複数のオブジェクトが重なった場合、最もz座標が小さい(~最も手前に描画されている)オブジェクトを選択
            //改善の余地あり(z座標を単純に比較するだけでは対応できない場合が今後発生する可能性があるため)
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

            //発射
            Fire(player, targetEnemiesList[index], bulletCreatePos_playerLocal, bulletSpd);
        }
    }


    /// <summary>
    /// マウスカーソルを敵に合わせているか判定
    /// </summary>
    /// <param name="mousePos">マウスカーソルのスクリーン座標</param>
    /// <param name="enemyPos">敵のスクリーン座標</param>
    /// <returns></returns>
    bool IsRockON(Vector2 mousePos, Vector2 enemyPos)
    {
        //Debug.Log((mousePos - enemyPos).magnitude);
        return (mousePos - enemyPos).magnitude <= distError;
    }


    /// <summary>
    /// 弾丸を敵の位置に発射する
    /// </summary>
    /// <param name="player">プレイヤーのゲームオブジェクト</param>
    /// <param name="enemy">敵のゲームオブジェクト</param>
    /// <param name="createPos">弾丸を作成する位置座標（プレイヤーを原点としたローカル座標）</param>
    /// <param name="bulletSpd">弾丸の速度</param>
    void Fire(GameObject player, GameObject enemy, Vector3 createPos, float bulletSpd)
    {
        //弾丸を生成
        GameObject bl = Instantiate(bullet);

        //座標を修正
        bl.transform.position = player.transform.position + createPos;

        bl.GetComponent<ControlBullet>().SetValues(enemy, bulletSpd, gameObject, power);
    }


    /// <summary>
    /// enemiesの要素を削除
    /// </summary>
    /// <param name="enemy">削除する要素</param>
    public void DestroyEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
