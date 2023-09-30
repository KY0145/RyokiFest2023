using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ベクトル計算用のクラス
/// </summary>
public class CalcVector : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが向いている方向のベクトルを返す
    /// </summary>
    /// <param name="yDeg">y軸の度数法での角度(=プレイヤーが向いている方向)</param>
    /// <param name="mag">返すベクトルの大きさ</param>
    /// <param name="playerVector3">プレイヤーの速度ベクトル</param>
    public static Vector3 ReturnDirection(float yDeg, float mag, Vector3 playerVector3)
    {
        yDeg = yDeg * Mathf.PI / 180;

        return new Vector3(Mathf.Sin(yDeg) * mag, playerVector3.y, Mathf.Cos(yDeg) * mag);
    }

    /// <summary>
    /// ベクトルをy方向に回転
    /// </summary>
    /// <param name="vec">回転させるベクトル</param>
    /// <param name="yRad">回転させる角度(ラジアン)</param>
    public static Vector3 Rotate(Vector3 vec, float yRad) //オーバーロード
    {
        return new Vector3(vec.x * Mathf.Cos(yRad) - vec.z * Mathf.Sin(yRad), vec.y, vec.z * Mathf.Cos(yRad) + vec.x * Mathf.Sin(yRad));
    }

}
