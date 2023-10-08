using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// CreateAssetMenu属性を使用することで`Assets > Create > ScriptableObjects > CreasteEnemyParamAsset`という項目が表示される
// それを押すとこの`EnemyParamAsset`が`EnemyParam`という名前でアセット化されてassetsフォルダに入る
[CreateAssetMenu(fileName = "EnemyParam", menuName = "ScriptableObjects/CreateEnemyParamAsset")]
public class EnemyParam : ScriptableObject
{
    [Header("エネミーの初期HP")]
    public float HP = 3.0f;

    [Header("攻撃力")]
    public float power = 1f;

    [Header("プレイヤーと衝突時に与えるダメージ")]
    public float bodyPower = 0f;

    [Header("弾丸生成位置")]
    public Vector3 createPos;

    [Header("弾丸の速さ")]
    public float bulletSpd;

    [Header("弾丸が追尾する")]
    public bool isTracing;

    [Header("弾丸が追尾する場合、弾丸が生成されてから何フレーム追尾するか")]
    public int tracingFrames;

    [Header("弾丸")]
    public GameObject bullet;


    [Header("見えるようになる距離")]
    public float spawnDis;

    [Header("行動を始める距離")]
    public float doDis;

    [Header("加える力の大きさ")]
    public float force;

    [Header("次の地点に進み始める距離")]
    public float rotateDis;

    [Header("制限速度")]
    public float limitVelo;

    [Header("攻撃する回数")]
    public int roopCount;

    [Header("x座標の範囲（最小値,最大値）")]
    public float[] x_range = new float[2];

    [Header("y座標の範囲（最小値,最大値）")]
    public float[] y_range = new float[2];

    [Header("z座標の範囲（最小値,最大値）")]
    public float[] z_range = new float[2];

    [Header("やられたときに加算されるスコア")]
    public float score;

}
