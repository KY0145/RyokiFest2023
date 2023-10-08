using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float score;

    private void Start()
    {
        score = 0;
    }


    public void AddScore(float addend)
    {
        score += addend;
    }
}
