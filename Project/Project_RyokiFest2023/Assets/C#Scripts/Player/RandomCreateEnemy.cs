using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


/// <summary>
/// Rを押すとエネミーをランダムな位置に作成
/// </summary>
public class RandomCreateEnemy : MonoBehaviour
{
    //[SerializeField] GameObject mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;

    [Header("x座標の範囲（最小値,最大値）")]
    [SerializeField]
    float[] x_range = new float[2];

    [Header("y座標の範囲（最小値,最大値）")]
    [SerializeField]
    float[] y_range = new float[2];

    [Header("z座標の範囲（最小値,最大値）")]
    [SerializeField]
    float[] z_range = new float[2];

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(CreateEnemyCoroutine(30, 2));
        }
    }

    Vector3 RandomPos(GameObject standardObj)
    {
        float x = Random.Range(x_range[0], x_range[1]);
        float y = Random.Range(y_range[0], y_range[1]);
        float z = Random.Range(z_range[0], z_range[1]);

        //Vector3 result = CalcVector.Rotate(new Vector3(x, y, z), -standardObj.transform.eulerAngles.y * Mathf.PI / 180);

        return new Vector3(x, y, z) + standardObj.transform.position;
    }

    void CreateEnemy()
    {
        var e = Instantiate(enemy);
        e.transform.position = RandomPos(player);
        GetComponent<MouseLockOnShooting>().enemies.Add(e);

        e.GetComponent<ControlEnemy>().player = player;
    }

    IEnumerator CreateEnemyCoroutine(int counts, float delaySeconds)
    {
        for (int i = 0; i < counts; i++)
        {
            CreateEnemy();
            yield return new WaitForSeconds(delaySeconds);
        }
    }
}
