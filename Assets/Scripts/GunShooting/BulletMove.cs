using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // 力を加えたかどうか
    bool powerAddFlag = false;

    // 弾丸のRigidbody
    Rigidbody _rigidbody;

    // 加える力
    [SerializeField]
    Vector3 addPower = new Vector3(0, 0, 20.0f);

    // 正面方向
    Vector3 frontVec;

    // Start is called before the first frame update
    void Start()
    {
        // 弾丸のRigidbodyを取得
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 弾丸は呼び出されたらすぐに移動を開始する
        if (!powerAddFlag)
        {
            // 一度しか実行させない
            powerAddFlag = true;

            // 射撃座標オブジェクトの正面方向のベクトルを取得
            frontVec = this.transform.forward.normalized;

            _rigidbody.AddForce(frontVec * addPower.magnitude, ForceMode.Impulse);
        }
    }
}
