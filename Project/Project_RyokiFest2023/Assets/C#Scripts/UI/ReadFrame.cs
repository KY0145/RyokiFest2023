using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadFrame : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private GameObject player;


    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.maxValue = 6;
        slider.value = player.GetComponent<MouseLockOnShooting>().frame + 6;
    }
}
