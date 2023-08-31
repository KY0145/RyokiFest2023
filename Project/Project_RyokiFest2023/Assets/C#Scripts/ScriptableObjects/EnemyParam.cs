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
}
