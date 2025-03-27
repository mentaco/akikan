using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    int limit_dm = 3;

    [SerializeField]
    float blown_force = 700;

    [SerializeField]
    private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision c)
    {
        limit_dm--;
        if (limit_dm < 1)
        {
            if (c.gameObject.CompareTag("Bullet"))
            {
                // 吹き飛ばし処理
                Vector3 direction = c.relativeVelocity.normalized * blown_force;
                Rigidbody p_rigidbody = transform.parent.GetComponent<Rigidbody>();
                p_rigidbody.AddForce(direction.x, blown_force / 2, direction.z, ForceMode.Impulse);

                // 爆発のエフェクト
                Instantiate(particle, c.transform.position, Quaternion.identity);

                // 非表示処理
                this.gameObject.SetActive(false);
            }
        }
    }
}
