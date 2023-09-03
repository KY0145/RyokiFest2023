using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロックオンカーソルにアタッチ
/// </summary>
public class TrackingMouseCursor : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;


    void Update()
    {
        var targetEnemiesList = player.GetComponent<MouseLockOnShooting>().targetEnemiesList;

        Vector2 screenPos;

        //ロックオンしているとき
        if (targetEnemiesList.Count != 0)
        {
            screenPos = mainCamera.WorldToScreenPoint(targetEnemiesList[0].transform.position);
        }
        else
        {
            screenPos = Input.mousePosition;
        }

        //照準のUIの位置を計算
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>().parent.GetComponent<RectTransform>(), screenPos, null, out var UIPos);

        transform.localPosition = UIPos;
    }
}
