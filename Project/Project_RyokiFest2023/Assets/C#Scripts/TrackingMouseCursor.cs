using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロックオンカーソルにアタッチ
/// </summary>
public class TrackingMouseCursor : MonoBehaviour
{
    [SerializeField] Camera camera;


    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>().parent.GetComponent<RectTransform>(), Input.mousePosition, null, out var UIPos);

        transform.localPosition = UIPos;
    }
}
