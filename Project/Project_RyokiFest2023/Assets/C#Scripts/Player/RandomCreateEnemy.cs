using System.Collections;
using UnityEngine;


/// <summary>
/// Rを押すとエネミーをランダムな位置に作成
/// </summary>
public class RandomCreateEnemy : MonoBehaviour
{
    //[SerializeField] GameObject mainCamera;
    [SerializeField] GameObject player;

    [SerializeField] GameObject scoreManager;

    //空中の敵
    [SerializeField] GameObject[] airEnemies;
    [SerializeField] EnemyParam airEnemyParam;

    //地上の敵
    [SerializeField] GameObject[] landEnemies;
    [SerializeField] EnemyParam landEnemyParam;



    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(CreateEnemyCoroutine(airEnemies, airEnemyParam, 30, 2));
            StartCoroutine(CreateEnemyCoroutine(landEnemies, landEnemyParam, 30, 10));
        }
    }

    Vector3 RandomPos(GameObject standardObj, float[] x_range, float[] y_range, float[] z_range)
    {
        float x = Random.Range(x_range[0], x_range[1]);
        float y = Random.Range(y_range[0], y_range[1]);
        float z = Random.Range(z_range[0], z_range[1]);

        //Vector3 result = CalcVector.Rotate(new Vector3(x, y, z), -standardObj.transform.eulerAngles.y * Mathf.PI / 180);

        return new Vector3(x, y, z) + standardObj.transform.position;
    }

    void CreateEnemy(GameObject[] enemies, EnemyParam enemyParam)
    {
        var e = Instantiate(enemies[Random.Range(0, enemies.Length)]);
        e.transform.position = RandomPos(player, enemyParam.x_range, enemyParam.y_range, enemyParam.z_range);
        GetComponent<MouseLockOnShooting>().enemies.Add(e);

        e.GetComponent<ControlEnemy>().player = player;
        e.GetComponent<ControlEnemy>().scoreManager = scoreManager;
    }

    IEnumerator CreateEnemyCoroutine(GameObject[] enemies, EnemyParam enemyParam, int counts, float delaySeconds)
    {
        for (int i = 0; i < counts; i++)
        {
            CreateEnemy(enemies, enemyParam);
            yield return new WaitForSeconds(delaySeconds);
        }
    }
}
