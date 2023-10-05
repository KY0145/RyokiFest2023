using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlHPbar : MonoBehaviour
{
    private Slider HPBarSlider;
    [SerializeField] private GameObject player;

    void Start()
    {
        HPBarSlider = GetComponent<Slider>();
    }

    void Update()
    {
        HPBarSlider.maxValue = player.GetComponent<ControlPlayer>().maxHP;
        HPBarSlider.value = player.GetComponent<ControlPlayer>().HP;
    }
}
