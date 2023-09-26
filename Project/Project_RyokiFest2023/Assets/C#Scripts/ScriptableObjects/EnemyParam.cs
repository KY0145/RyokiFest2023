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

    [Header("弾丸生成位置")]
    public Vector3 createPos;

    [Header("弾丸の速さ")]
    public float bulletSpd;

    [Header("弾丸が追尾する")]
    public bool isTracing;

    [Header("弾丸")]
    public GameObject bullet;
}
