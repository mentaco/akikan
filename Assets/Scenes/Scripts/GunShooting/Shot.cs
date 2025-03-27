using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour {
  // 弾丸の情報を取得
  [SerializeField]
  GameObject BulletObj;

  // 弾丸の向きを保持しているオブジェクト
  [SerializeField]
  GameObject shotPosObj;

  // 発射間隔
  [SerializeField]
  float shotMaxCoolTime = 5.0f;

  // 現在のクールタイム
  float shotCoolTime = 0;

  // Start is called before the first frame update
  void Start() {}

  // Update is called once per frame
  void Update() {
    if (Input.GetKey(KeyCode.Z)) {
      // クールタイムが 0 ならば射撃可能
      if (shotCoolTime <= 0)
      {
          // 弾丸をインスタンス化
          GameObject bullet = Instantiate(BulletObj, shotPosObj.transform.position,
                                      shotPosObj.transform.rotation);

          // クールタイムを更新
          shotCoolTime = shotMaxCoolTime;
      }
    }
  }

  private void FixedUpdate()
  {
      // クールタイムを減少
      shotCoolTime--;

      // 0 以下にしない
      if (shotCoolTime <= 0)
      {
          shotCoolTime = 0;
      }
  }
}
