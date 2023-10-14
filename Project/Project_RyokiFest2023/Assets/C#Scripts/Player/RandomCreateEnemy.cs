using System.Collections;
using UnityEngine;


/// <summary>
/// Rを押すとエネミーをランダムな位置に作成
/// </summary>
public class RandomCreateEnemy : MonoBehaviour
{
    //[SerializeField] GameObject mainCamera;
    [SerializeField] GameObject player;

    [Header("プレイヤーの座標の基準となるオブジェクト")]
    [SerializeField] GameObject stdPosPlayer;

    [SerializeField] GameObject scoreManager;

    //空中の敵
    [SerializeField] GameObject[] airEnemies;

    //地上の敵
    [SerializeField] GameObject[] landEnemies;



    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(CreateEnemyCoroutine(airEnemies, 2, 45, 2));
            StartCoroutine(CreateEnemyCoroutine(landEnemies, 1, 30, 3));
        }
    }

    /// <summary>
    /// standardObjを中心としたローカル座標で敵を生成
    /// ただしy座標はグローバル座標として扱う
    /// </summary>
    /// <param name="standardObj">ローカル座標の基準となるオブジェクト</param>
    /// <param name="x_range">x(ローカル座標)の範囲</param>
    /// <param name="y_range">y(グローバル座標)の範囲</param>
    /// <param name="z_range">z(ローカル座標)の範囲</param>
    /// <returns></returns>
    Vector3 RandomPos(GameObject standardObj, float[] x_range, float[] y_range, float[] z_range)
    {
        float x = Random.Range(x_range[0], x_range[1]);
        float y = Random.Range(y_range[0], y_range[1]);
        float z = Random.Range(z_range[0], z_range[1]);

        //Vector3 result = CalcVector.Rotate(new Vector3(x, y, z), -standardObj.transform.eulerAngles.y * Mathf.PI / 180);

        return new Vector3(x, y, z) + new Vector3(standardObj.transform.position.x, 0, standardObj.transform.position.z);
    }

    void CreateEnemy(GameObject[] enemies)
    {
        var e = Instantiate(enemies[Random.Range(0, enemies.Length)]);
        var enemyParam = e.GetComponent<ControlEnemy>().enemyParam;
        e.transform.position = RandomPos(player, enemyParam.x_range, enemyParam.y_range, enemyParam.z_range);
        GetComponent<MouseLockOnShooting>().enemies.Add(e);

        e.GetComponent<ControlEnemy>().player = player;
        e.GetComponent<ControlEnemy>().stdPosPlayer = stdPosPlayer;
        e.GetComponent<ControlEnemy>().scoreManager = scoreManager;
    }

    IEnumerator CreateEnemyCoroutine(GameObject[] enemies, int enemyCounts, int roopCounts, float delaySeconds)
    {
        for (int i = 0; i < roopCounts; i++)
        {
            for(int i1 = 0; i1 < enemyCounts; i1++)
            {
                CreateEnemy(enemies);
            }
            yield return new WaitForSeconds(delaySeconds);
        }
    }
}
