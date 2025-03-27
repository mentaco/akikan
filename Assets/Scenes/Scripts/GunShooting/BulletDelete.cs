using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDelete : MonoBehaviour {
  // 他オブジェクトに接触したかどうか
  bool otherObjHitFlag = false;

  // 存在時間の指定
  [SerializeField]
  float aliveTime = 10;

  // 限界距離の設定
  [SerializeField]
  float limitMoveDist = 50.0f;

  // 発射時の座標
  Vector3 shotPos;

  // 現在の座標
  Vector3 currentPos;

  // Start is called before the first frame update
  void Start() { shotPos = this.transform.position; }

  // Update is called once per frame
  void Update() {
    // 他オブジェクトと接触した場合
    MoveOverDestroy();
  }

  private void FixedUpdate() {
    // 一定距離を移動した場合
    HitDeleteUpdate();
  }

  // 接触時に呼び出される
  void OnCollisionEnter(Collision collision) {
    // レイヤーが "Player" or "Bullet" であれば無視
    if (collision.gameObject.layer == LayerMask.NameToLayer("Player") |
        collision.gameObject.layer == LayerMask.NameToLayer("Bullet")) {
      return;
    }

    otherObjHitFlag = true;
  }

  // 接触したら削除
  void HitDeleteUpdate() {
    // 他オブジェクトに接触した場合
    if (otherObjHitFlag) {
      // 時間を減少させる
      aliveTime--;

      // 指定した時間が経過していたらオブジェクトを削除
      if (aliveTime <= 0) {
        // 数値を固定
        aliveTime = 0;

        // 削除
        DestroyThisObj();
      }
    }
  }

  void MoveOverDestroy() {
    // 現在の座標を取得
    currentPos = this.transform.position;

    // 移動距離
    float moveDist = Vector3.Distance(shotPos, currentPos);

    // 移動距離が限界距離を超えたら削除
    if (moveDist >= limitMoveDist) {
      DestroyThisObj();
    }
  }

  void DestroyThisObj() {
    // 削除済みであれば何もしない
    if (!this.gameObject) {
      return;
    }

    Destroy(this.gameObject);
  }
}
