using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet; //íeä€

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }


    void Fire()
    {
        GameObject bl = Instantiate(bullet);

        bl.GetComponent<Rigidbody>().velocity = new Vector3 (0, 1, 1);
    }
}
