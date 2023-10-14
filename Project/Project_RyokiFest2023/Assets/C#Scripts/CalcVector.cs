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
    public static Vector3 Rotate(Vector3 vec, float yRad)
    {
        return new Vector3(vec.x * Mathf.Cos(yRad) - vec.z * Mathf.Sin(yRad), vec.y, vec.z * Mathf.Cos(yRad) + vec.x * Mathf.Sin(yRad));
    }

    /// <summary>
    /// 2つの2次元ベクトルがなす角を計算
    /// </summary>
    /// <param name="a">1つ目のベクトル</param>
    /// <param name="b">2つ目のベクトル</param>
    /// <param name="isCCW">aから反時計回りを正に[0,2pi]の範囲で返すときはtrue、ベクトルがなす角の小さいほうを返すときはfalse</param>
    /// <returns>角度をラジアンで返す</returns>
    public static float CalcVecRad(Vector2 a, Vector2 b, bool isCCW)
    {
        float rad;

        if (a != b && a.magnitude * b.magnitude != 0)
        {
            rad = Mathf.Acos(Vector2.Dot(a, b) / (a.magnitude * b.magnitude));
        }
        else
        {
            rad = 0f;
        }

        if (!isCCW) return rad;
        else
        {
            if (Vector3.Cross(a, b).z >= 0)
            {
                return rad;
            }
            else return 2 * Mathf.PI - rad;
        }
    }
}
