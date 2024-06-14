using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grenade : Interactable
{

    public float delay = 3f;

    float timer;

    public GameObject effect;

    public float explodeRadius = 50f;

    public float explodeDamage = 70f;

    public bool thrown = false;

    bool exploded = false;

    private void GrenadeExplode()
    {
        Collider[] entities = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider collider in entities) 
        { 
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<Enemy>().enemyHealth -= explodeDamage;
            }
            else if (collider.gameObject.tag == "Player")
            {
                Debug.Log("ok");
                GameManager.instance.playerHealth -= explodeDamage;
            }
        }

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (thrown)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (!exploded)
                {
                    GrenadeExplode();
                    exploded = true;
                }

            }
        }
    }

}
