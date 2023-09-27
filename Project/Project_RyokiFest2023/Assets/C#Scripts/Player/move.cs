using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class move : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float velocity;
    [SerializeField] private float accelerator;
    [SerializeField] private float rotateSpd;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        //左右移動
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
        {
            transform.eulerAngles -= new Vector3(0,rotateSpd,0);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, rotateSpd, 0);
        }

        //前方移動
        rb.velocity = ReturnDirection(transform.eulerAngles.y, velocity + ac);
    }


    /// <summary>
    /// プレイヤーが向いている方向のベクトルを返す
    /// </summary>
    /// <param name="yDeg">y軸の度数法での角度(=プレイヤーが向いている方向)</param>
    /// <param name="mag">返すベクトルの大きさ</param>
    public static Vector3 ReturnDirection(float yDeg, float mag)
    {
        yDeg = yDeg * Mathf.PI / 180;

        return new Vector3(Mathf.Sin(yDeg), 0, Mathf.Cos(yDeg)) * mag;
    }
}
