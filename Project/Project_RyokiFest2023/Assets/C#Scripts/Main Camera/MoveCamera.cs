using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Vector3 movingPos;

    [SerializeField] private GameObject player;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.eulerAngles += new Vector3(0, player.transform.eulerAngles.y, 0);

        transform.position = player.transform.position + move.ReturnDirection(player.transform.eulerAngles.y, movingPos.z) + new Vector3(0, movingPos.y, 0);
    }
}
