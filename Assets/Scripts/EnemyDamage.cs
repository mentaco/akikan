using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    int limit_dm = 3;

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
        if (limit_dm < 1)
        {
            if (c.gameObject.CompareTag("Bullet"))
            {
                Instantiate(particle, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            limit_dm--;
        }
    }
}
