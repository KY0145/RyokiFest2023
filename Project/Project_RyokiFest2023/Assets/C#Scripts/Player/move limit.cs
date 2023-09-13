using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movelimit : MonoBehaviour
{

    //x²•ûŒü‚ÌˆÚ“®”ÍˆÍ‚ÌÅ¬’l
    [SerializeField] private float _minX = -1;

    //x²•ûŒü‚ÌˆÚ“®”ÍˆÍ‚ÌÅ‘å’l
    [SerializeField] private float _maxX = 1;



    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;

        // x²•ûŒü‚ÌˆÚ“®”ÍˆÍ‚Ì§ŒÀ
        pos.x = Mathf.Clamp(pos.x, _minX, _maxX);

        transform.position = pos;
    }
}
