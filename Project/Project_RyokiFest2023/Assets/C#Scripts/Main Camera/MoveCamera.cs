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

    void Update()
    {
        //transform.eulerAngles += new Vector3(0, player.transform.eulerAngles.y, 0);

        //transform.position = player.transform.position + CalcVector.ReturnDirection(player.transform.eulerAngles.y, movingPos.z, movingPos);

        transform.position = player.transform.position + movingPos;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }
}
