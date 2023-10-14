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

    [SerializeField] private GameObject explosion;

    /// <summary>
    /// 2回以上爆発が起きないようにするbool
    /// </summary>
    private bool oneTime = true;

    void Start()
    {
        HP = maxHP;
    }

    void Update()
    {
        //HPが0のとき、Rを押すとタイトルに戻る
        if (HP <= 0)
        {
            Vector3 v = GetComponent<Rigidbody>().velocity;
            v = new Vector3(0, v.y, 0);

            if (oneTime)
            {
                //プレイヤーの爆発
                explosion.GetComponent<ParticleSystem>().Play();
                oneTime = false;
            }

            //プレイヤーのやられアニメーション
            GetComponent<Animator>().SetBool("Death", true);

            //移動を制限
            GetComponent<movePlayer>().enabled = false;
            //攻撃を制限
            GetComponent<MouseLockOnShooting>().isEnd = false;

            //ゲームオーバーになったらリザルト画面を表示
            if (GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Player_DeathAnimation"
                && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.5)
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
