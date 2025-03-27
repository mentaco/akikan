using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_hpReduce : MonoBehaviour
{
    public Image HPGage;

    private int MAX_HP = 100;
    private int HP;

    void Start()
    {
        HP = MAX_HP;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemyAttack"))
        {
            HP -= 10;
            float percent = (float)HP / MAX_HP;
            HPGage.fillAmount = percent;
            Debug.Log("プレイヤーの攻撃、10のダメージ");

            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}