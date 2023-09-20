using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private Rigidbody rb;
    public float velocity;
    public float accelerator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //à⁄ìÆë¨ìxÇíºê⁄ïœçXÇ∑ÇÈ
        rb.velocity = new Vector3(0, 0, 0);

        float ac = 0;

        if (Input.GetKey(KeyCode.Space))
        {
            ac = accelerator;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            rb.velocity = new Vector3(-(velocity+ac), 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector3(velocity+ac, 0, 0);
        }
       
    }
}
