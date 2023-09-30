using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public float HP;

    void Start()
    {
        
    }

    void Update()
    {
        if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
