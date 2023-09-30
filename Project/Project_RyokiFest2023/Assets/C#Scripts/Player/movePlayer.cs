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

    [Header("タイヤが回転する角度")]
    [SerializeField] private float tyreRotatDeg;

    /// <summary>
    /// 例えば60ならば、基準角度を0として左右60度までに制限される
    /// 計算誤差の影響か179.9度では見た目上制限がなくなる
    /// また、180度以上では角度が固定されてしまう
    /// </summary>
    [Header("制限角度(度数法)")]
    [SerializeField] private float limit_Deg;

    /// <summary>
    /// 前側のタイヤ
    /// </summary>
    private GameObject leftFrontTyre;
    private GameObject rightFrontTyre;

    private Vector3 leftFrontDeg;
    private Vector3 rightFrontDeg;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        standardDeg = transform.eulerAngles.y;

        //タイヤを探す
        leftFrontTyre = transform.Find("左前タイヤ").gameObject;
        rightFrontTyre = transform.Find("右前タイヤ").gameObject;

        //タイヤの角度を保存
        leftFrontDeg = leftFrontTyre.transform.eulerAngles;
        rightFrontDeg = rightFrontTyre.transform.eulerAngles;
    }

    void FixedUpdate()
    {
        //加速用ベクトル
        float ac = 0;

        //加速用ベクトルに値を設定
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {
            ac = accelerator;
        }

        //タイヤ回転用変数
        float tyreRotation = 0;

        //方向変更
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
        {
            transform.eulerAngles -= new Vector3(0,rotateSpd,0);
            tyreRotation -= tyreRotatDeg;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, rotateSpd, 0);
            tyreRotation += tyreRotatDeg;
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
            tyreRotation = 0;
        }
        else if (isInRange(transform.eulerAngles.y, clamp(standardDeg - 180), clamp(standardDeg - limit_Deg)))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, standardDeg - limit_Deg, transform.eulerAngles.z);
            tyreRotation = 0;
        }

        //タイヤの角度変更
        leftFrontTyre.transform.eulerAngles = leftFrontDeg + new Vector3(0, tyreRotation, 0) + new Vector3(0, transform.eulerAngles.y, 0);
        rightFrontTyre.transform.eulerAngles = rightFrontDeg + new Vector3(0, tyreRotation, 0) + new Vector3(0, transform.eulerAngles.y, 0);

        //前方移動
        rb.velocity = CalcVector.ReturnDirection(transform.eulerAngles.y, velocity + ac, rb.velocity);
    }
}
