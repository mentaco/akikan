using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour {
  // 当たった時に呼ばれる関数
    void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.tag == "enemyAttack"){
        Debug.Log("Hit"); // ログを表示する
      }
    }
}