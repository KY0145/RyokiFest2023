using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class movePlayer : MonoBehaviour
{
    private Rigidbody rb;

    [Header("通常速さ")]
    [SerializeField] private float velocity;

    [Header("加速時に追加する速さ")]
    [SerializeField] private float accelerator;

    [Header("回転速度")]
    [SerializeField] private float rotateSpd;

    [Header("基準角度")]
    public float standardDeg;

    /// <summary>
    /// 例えば60ならば、基準角度を0として左右60度までに制限される
    /// 計算誤差の影響か179.9度では見た目上制限がなくなる
    /// また、180度以上では角度が固定されてしまう
    /// </summary>
    [Header("制限角度(度数法)")]
    [SerializeField] private float limit_Deg;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        standardDeg = transform.eulerAngles.y;
    }

    void Update()
    {
        //加速用ベクトル
        float ac = 0;

        //加速用ベクトルに値を設定
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {
            ac = accelerator;
        }

        //方向変更
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
        {
            transform.eulerAngles -= new Vector3(0,rotateSpd,0);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, rotateSpd, 0);
        }

        //方向制限
        float clamp(float a)
        {
            if (a < 0)
            {
                return a + 360;
            }
            else if (a > 360)
            {
                return a - 360;
            }
            else
            {
                return a;
            }
        }

        bool isInRange(float value, float min, float max)
        {
            if (min < max)
            {
                return min <= value && value <= max;
            }
            else
            {
                return (0 <= value && value <= max) || (min <= value && value <= 360);
            }
        }


        if (isInRange(transform.eulerAngles.y, clamp(standardDeg + limit_Deg), clamp(standardDeg + 180)))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, standardDeg + limit_Deg, transform.eulerAngles.z);
        }
        else if (isInRange(transform.eulerAngles.y, clamp(standardDeg - 180), clamp(standardDeg - limit_Deg)))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, standardDeg - limit_Deg, transform.eulerAngles.z);
        }

        //前方移動
        rb.velocity = ReturnDirection(transform.eulerAngles.y, velocity + ac, rb.velocity);
    }


    /// <summary>
    /// プレイヤーが向いている方向のベクトルを返す
    /// </summary>
    /// <param name="yDeg">y軸の度数法での角度(=プレイヤーが向いている方向)</param>
    /// <param name="mag">返すベクトルの大きさ</param>
    /// <param name="playerPosition">プレイヤーの速度ベクトル</param>
    public static Vector3 ReturnDirection(float yDeg, float mag, Vector3 playerVector3)
    {
        yDeg = yDeg * Mathf.PI / 180;

        return new Vector3(Mathf.Sin(yDeg) * mag, playerVector3.y, Mathf.Cos(yDeg) * mag);
    }
}
