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

    [Header("この値よりy座標が小さいと毎フレームダメージを受ける")]
    [SerializeField] private float y_min;

    [SerializeField] private GameObject ResultPanel;

    void Start()
    {
        HP = maxHP;
    }

    void Update()
    {
        //HPが0のとき、Rを押すとタイトルに戻る
        if (HP <= 0)
        {
            //プレイヤーのやられアニメーション
            GetComponent<Animator>().SetBool("Death", true);

            //移動を制限
            GetComponent<movePlayer>().enabled = false;

            //ゲームオーバーになったらリザルト画面を表示
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.5)
            {
                ResultPanel.SetActive(true);
            }
        }

        if (transform.position.y < y_min)
        {
            HP--;
        }

        hpText.SetText("HP : " + HP);
    }
}
