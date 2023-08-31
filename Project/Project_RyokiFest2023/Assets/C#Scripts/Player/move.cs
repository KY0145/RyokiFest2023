using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private Rigidbody rb;
    public float velocity;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //ˆÚ“®‘¬“x‚ğ’¼Ú•ÏX‚·‚é
        rb.velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            rb.velocity = new Vector3(-velocity, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector3(velocity, 0, 0);
        }
    }
}
