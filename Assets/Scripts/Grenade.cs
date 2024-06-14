using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grenade : Interactable
{
    [SerializeField]
    float grenadeDistance;

    public float delay = 3f;

    float timer;

    public GameObject effect;

    public float explodeRadius = 50f;

    public float explodeDamage = 70f;

    public bool thrown = false;

    bool set = true;

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
            if (set)
            {
                gameObject.transform.parent = null;
                gameObject.GetComponent<SphereCollider>().enabled = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().AddForce(grenadeDistance * GameManager.instance.playerCamera.transform.forward);
                GameManager.instance.grenadeCount -= 2;
                GameManager.instance.currentGrenade = null;
                GameManager.instance.currentEquippable = null;
                set = false;
                Debug.Log(GameManager.instance.grenadeCount);
            }
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

    public override void Interact(Player thePlayer)
    {
        if(GameManager.instance.currentEquippable != null)
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }
        GameManager.instance.grenadeCount += 1;
        gameObject.transform.SetParent(GameManager.instance.playerCamera.transform, false);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        GameManager.instance.currentGrenade = gameObject;
        GameManager.instance.currentEquippable = gameObject;
        GameManager.instance.currentEquippable.SetActive(true);
        gameObject.transform.position = GameManager.instance.equipPosition.transform.position;
        gameObject.transform.eulerAngles = GameManager.instance.equipPosition.transform.eulerAngles;
        Debug.Log("negga");
    }
}
