using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPlayer : MonoBehaviour
{
    public float maxHP;
    [HideInInspector] public float HP;
    [SerializeField] private TMP_Text hpText;

    void Start()
    {
        HP = maxHP;
    }

    void Update()
    {
        //HPが0のとき、Rを押すとタイトルに戻る
        if (HP <= 0 && Input.GetKeyDown(KeyCode.R))
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("Title");
        }

        hpText.SetText("HP : " + HP);
    }
}
