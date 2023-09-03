using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カーソルの移動に合わせてカメラが若干動くようにするスクリプト
/// </summary>
public class CameraSwiveling: MonoBehaviour
{
     private ProcessDistanceCenter pdc;

    /// <summary>
    /// x軸の最大角度(度)
    /// </summary>
    [Header("x軸の最大角度(度)")]
    [SerializeField] private float xAng_max = 15;

    /// <summary>
    /// y軸の最大角度(度)
    /// </summary>
    [Header("y軸の最大角度(度)")]
    [SerializeField] private float yAng_max = 15;

    /*
     * 以下二つの変数はスクリーンの端を1,中心を0としている
     */

    /// <summary>
    /// 角度が変化しないカーソルの座標の範囲(x軸)
    /// </summary>
    [Header("角度が変化しないカーソルの座標の範囲(x軸)")]
    [SerializeField] private float unchangeRange_x = 0.25f;

    /// <summary>
    /// 角度が変化しないカーソルの座標の範囲(y軸)
    /// </summary>
    [Header("角度が変化しないカーソルの座標の範囲(y軸)")]
    [SerializeField] private float unchangeRange_y = 0.25f;


    void Update()
    {
        pdc = new ProcessDistanceCenter(xAng_max, yAng_max, unchangeRange_x, unchangeRange_y);
        transform.eulerAngles = pdc.ReturnCameraAng(Input.mousePosition);
    }
}


/// <summary>
/// 画面の中心線からの距離で処理
/// Processed by distance from screen center line
/// </summary>
public class ProcessDistanceCenter
{
    private float xAng_max;
    private float yAng_max;

    private float unchangeRange_x;
    private float unchangeRange_y;

    public ProcessDistanceCenter(float xAng_max, float yAng_max, float unchangeRange_x, float unchangeRange_y)
    {
        this.xAng_max = xAng_max;
        this.yAng_max = yAng_max;
        this.unchangeRange_x = unchangeRange_x;
        this.unchangeRange_y = unchangeRange_y;
    }

    public Vector2 ReturnCameraAng(Vector2 mousePos)
    {
        mousePos -= new Vector2(Screen.width / 2, Screen.height / 2);
        mousePos = new Vector2(mousePos.x, -mousePos.y);
        mousePos = ReturnMousePos(mousePos, unchangeRange_x, unchangeRange_y);

        float xAng = (mousePos.y / (Screen.height / 2)) * xAng_max;
        float yAng = (mousePos.x / (Screen.width / 2)) * yAng_max;

        return new Vector2(xAng, yAng);
    }

    // 0 < unchangeRange_x, unchangeRange_y <= 1 であることに注意
    private Vector2 ReturnMousePos(Vector2 mousePos, float unchangeRange_x, float unchangeRange_y)
    {
        if (unchangeRange_x <= 0 || unchangeRange_x > 1 ||  unchangeRange_y <= 0 || unchangeRange_y > 1)
        {
            Debug.LogWarning("値が範囲外です");
            return mousePos;
        }

        mousePos.x = clamp(mousePos.x, unchangeRange_x, Screen.width);
        mousePos.y = clamp(mousePos.y, unchangeRange_y, Screen.height);

        return mousePos;

        float clamp(float mousePosxy, float unchangeRangexy, int screen)
        {
            return Mathf.Sign(mousePosxy) * Mathf.Clamp(Mathf.Abs(mousePosxy) / unchangeRangexy - (screen / 2) * ((1 / unchangeRangexy) - 1), 0, screen / 2);
        }
    }
}
