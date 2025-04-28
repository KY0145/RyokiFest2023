using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;


/// <summary>
/// 弾丸を発射するときに、スクリーン上で敵にカーソルを合わせていると自動でその位置に向けて発射するようにするためのスクリプト
/// </summary>


public class MouseLockOnShooting : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private Camera targetCamera;

    /// <summary>
    /// ロックオンできる距離
    /// </summary>
    [Header("ロックオンできる距離")]
    [SerializeField] private float disLimit;

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

    [Header("ロックオンする敵の仮のリスト")]
    public List<GameObject> targetEnemiesList;

    /// <summary>
    /// プレイヤーがやられたあとに攻撃不可にする用
    /// </summary>
    public bool isEnd = true;

    public int frame = 0;

    //攻撃の種類を識別するための整数
    [SerializeField] private enum typeOfAttack
    {
        NORMAL
    }



    void FixedUpdate()
    {
        //Tを押すと発射方法切り替え（デバッグ用）
        if (Input.GetKeyDown(KeyCode.T))
        {
            isRensya = !isRensya;
        }

        Vector4 plane = EquationPlane(player.transform.position, player.transform.position + CalcVector.ReturnDirection(player.transform.eulerAngles.y + 90, 1, Vector3.zero), player.transform.position + new Vector3(0,1,0));

        targetEnemiesList = new List<GameObject>();

        Vector2 mousePos = Input.mousePosition; //マウスカーソルのスクリーン座標

        foreach (var enemy in enemies)
        {
            if (IsRockON(mousePos, enemy.transform.position, targetCamera.transform.position, plane))
            {
                targetEnemiesList.Add(enemy);
            }
        }

        
        if (targetEnemiesList.Count != 0 && isEnd)
        {
            //スクリーン座標で複数のオブジェクトが重なった場合、プレイヤーが向いている方向に垂直な平面との距離が最も小さいものを選択
            int index = 0;
            float dis = 0;
            for (int i = 0; i < targetEnemiesList.Count; i++)
            {
                if (i == 0)
                {
                    dis = Math.Abs(DisPlanePoint(plane, targetEnemiesList[i].transform.position));
                    index = i;
                }
                else
                {
                    if (dis > Math.Abs(DisPlanePoint(plane, targetEnemiesList[i].transform.position)))
                    {
                        dis = Math.Abs(DisPlanePoint(plane, targetEnemiesList[i].transform.position));
                        index = i;
                    }
                }
            }

            //発射
            if (isRensya)
            {
                Fire(player, targetEnemiesList[index], bulletCreatePos_playerLocal, bulletSpd, true, 1, (int)typeOfAttack.NORMAL);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Fire(player, targetEnemiesList[index], bulletCreatePos_playerLocal, bulletSpd, true, frame, (int)typeOfAttack.NORMAL);
                }
            }
        }

        //フレームを一つ増やす
        frame++;
    }


    /// <summary>
    /// マウスカーソルを敵に合わせているか判定
    /// </summary>
    /// <param name="mouseScreenPos">マウスカーソルのスクリーン座標</param>
    /// <param name="enemyWorldPos">敵のワールド座標</param>
    /// <param name="cameraWorldPos">カメラのワールド座標</param>
    /// <returns></returns>
    bool IsRockON(Vector2 mouseScreenPos, Vector3 enemyWorldPos, Vector3 cameraWorldPos, Vector4 plane)
    {
        //敵のワールド座標をスクリーン座標に変換
        Vector2 enemyScreenPos = targetCamera.WorldToScreenPoint(enemyWorldPos);
        
        mouseScreenPos = new Vector2(mouseScreenPos.x / Camera.main.pixelWidth, mouseScreenPos.y / Camera.main.pixelWidth);
        enemyScreenPos = new Vector2(enemyScreenPos.x / Camera.main.pixelWidth, enemyScreenPos.y / Camera.main.pixelWidth);

        bool isScreen = (mouseScreenPos - enemyScreenPos).magnitude <= distError;
        bool isDis = Mathf.Abs(DisPlanePoint(plane, enemyWorldPos)) - Mathf.Abs(DisPlanePoint(plane, cameraWorldPos)) <= disLimit;

        return isScreen && isDis;
    }


    /// <summary>
    /// 弾丸を敵の位置に発射する
    /// </summary>
    /// <param name="player">プレイヤーのゲームオブジェクト</param>
    /// <param name="enemy">敵のゲームオブジェクト</param>
    /// <param name="createPos">弾丸を作成する位置座標（プレイヤーを原点としたローカル座標）</param>
    /// <param name="bulletSpd">弾丸の速度</param>
    void Fire(GameObject player, GameObject enemy, Vector3 createPos, float bulletSpd, bool isTracing, int frame, int type)
    {
        if (frame <= 0)
        {
            return;
        }

        if (type == 0)
        {
            //弾丸を生成
            GameObject bl = Instantiate(bullet);

            //座標を修正
            bl.transform.position = player.transform.position + createPos;

            bl.GetComponent<ControlBullet>().SetValues(enemy, bulletSpd, gameObject, power, isTracing, int.MaxValue);

            //0.1秒(=6フレーム)クールダウン
            this.frame = -6;
        }
        else if (type == 1)
        {

        }
        else if (type == 2)
        {
             
        }
    }


    /// <summary>
    /// enemiesの要素を削除
    /// </summary>
    /// <param name="enemy">削除する要素</param>
    public void DestroyEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }


    /// <summary>
    /// 平面の方程式を導出する
    /// </summary>
    /// <param name="a">平面上の1点</param>
    /// <param name="b">平面上の1点</param>
    /// <param name="c">平面上の1点</param>
    /// <returns>ax+by+cz+d=0のa, b, c, dを格納したVector4</returns>
    Vector4 EquationPlane(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;

        Vector3 abc = Vector3.Cross(ab, ac);

        return new Vector4(abc.x, abc.y, abc.z, -(abc.x * a.x + abc.y * a.y + abc.z * a.z));
    }


    /// <summary>
    /// 平面と点の符号付き距離
    /// </summary>
    /// <param name="plane">ax+by+cz+d=0のa, b, c, d</param>
    /// <param name="point"></param>
    /// <returns>平面と点の符号付き距離</returns>
    float DisPlanePoint(Vector4 plane, Vector3 point) //Distance between plane and point
    {
        return (plane.x * point.x + plane.y * point.y + plane.z * point.z + plane.w) / Mathf.Sqrt(Mathf.Pow(plane.x, 2) + Mathf.Pow(plane.y, 2) + Mathf.Pow(plane.z, 2));
    }
}
