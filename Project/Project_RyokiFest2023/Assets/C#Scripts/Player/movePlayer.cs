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

    [Header("ジャンプ時に与える力の大きさ")]
    [SerializeField] private float jumpForce;

    [Header("y座標の上限値")]
    [SerializeField] private float y_limit;

    [Header("基準角度")]
    public float standardDeg;


    /// <summary>
    /// 例えば60ならば、基準角度を0として左右60度までに制限される
    /// 計算誤差の影響か179.9度では見た目上制限がなくなる
    /// また、180度以上では角度が固定されてしまう
    /// </summary>
    [Header("制限角度(度数法)")]
    [SerializeField] private float limit_Deg;

    private Animator animator;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        standardDeg = transform.eulerAngles.y;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //アニメーション遷移用bool
        bool right = false;
        bool left = false;

        //加速用ベクトル
        float ac = 0;

        //加速用ベクトルに値を設定
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            ac = accelerator;
        }

        //方向変更
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
        {
            transform.eulerAngles -= new Vector3(0,rotateSpd,0);
            left = true;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, rotateSpd, 0);
            right = true;
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
            right = false;
        }
        else if (isInRange(transform.eulerAngles.y, clamp(standardDeg - 180), clamp(standardDeg - limit_Deg)))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, standardDeg - limit_Deg, transform.eulerAngles.z);
            left = false;
        }

        //タイヤの角度変更
        animator.SetBool("Right", right);
        animator.SetBool("Left", left);

        //前方移動
        rb.velocity = CalcVector.ReturnDirection(transform.eulerAngles.y, velocity + ac, rb.velocity);


        //ジャンプ
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        //y座標制限
        if (transform.position.y > y_limit)
        {
            transform.position = new Vector3(transform.position.x, y_limit, transform.position.z);
            rb.AddForce(new Vector3(0, -jumpForce, 0));
        }
    }
}
