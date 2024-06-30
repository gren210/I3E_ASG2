using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grenade : Interactable
{
    public string interactText;

    [SerializeField]
    float grenadeDistance;

    public float delay = 3f;

    float timer;

    public ParticleSystem effect;

    public float explodeRadius = 50f;

    public float explodeDamage = 70f;

    [HideInInspector]
    public bool thrown = false;

    bool set = true;

    bool exploded = false;

    [SerializeField]
    AudioClip grenadeSound;

    [SerializeField]
    float shakeTimer;

    float shakeTimerStart;

    [SerializeField]
    float shakeIntensity;

    [SerializeField]
    float shakeFrequency;

    [SerializeField]
    float swapThrowForce;

    public int iconIndex;

    private void GrenadeExplode()
    {
        effect.GetComponent<ParticleSystem>().Play();

        AudioSource.PlayClipAtPoint(grenadeSound, transform.position);

        Collider[] entities = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider collider in entities) 
        { 
            if (collider.gameObject.tag == "Enemy")
            {
                UpdateEnemyHealth(collider.gameObject, explodeDamage);
            }
            else if (collider.gameObject.tag == "Boss")
            {
                UpdateEnemyHealth(collider.gameObject, explodeDamage);
            }
            else if (collider.gameObject.tag == "Player" && !GameManager.instance.isImmune)
            {
                GameManager.instance.playerHealth -= explodeDamage;
                GameManager.instance.UpdateHealth();
            }
        }
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
                GameManager.instance.throwSound.Play();
                GameManager.instance.currentGrenadeIcon.SetActive(false);
                GameManager.instance.currentGrenadeIcon = null;
                gameObject.transform.parent = null;
                gameObject.GetComponent<SphereCollider>().enabled = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().AddForce(grenadeDistance * GameManager.instance.playerCamera.transform.forward);
                GameManager.instance.currentGrenade = null;
                GameManager.instance.currentEquippable = null;
                set = false;
            }
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (!exploded)
                {
                    GrenadeExplode();
                    exploded = true;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<MeshFilter>().mesh = null;
                    Destroy(gameObject,2f);
                    Destroy(effect, 2f);
                    ShakeCamera(shakeIntensity, shakeFrequency);
                    shakeTimerStart = shakeTimer;

                }

            }
        }
        if (shakeTimerStart > 0)
        {
            shakeTimerStart -= Time.deltaTime;
            if (shakeTimerStart <= 0f)
            {
                ShakeCamera(0f, 0f);
            }
        }
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        PickupCollectible(gameObject, iconIndex,GameManager.instance.currentGrenade);
    }

    protected override void PickupCollectible(GameObject collectible, int iconIndex, GameObject currentCollectible)
    {
        if (GameManager.instance.currentGrenade != null)
        {
            if (GameManager.instance.currentGrenade.activeSelf == false)
            {
                GameManager.instance.currentGrenade.SetActive(true);
            }
            GameManager.instance.currentGrenade.transform.parent = null;
            GameManager.instance.currentGrenade.GetComponent<SphereCollider>().enabled = true;
            GameManager.instance.currentGrenade.GetComponent<Rigidbody>().isKinematic = false;
            GameManager.instance.currentGrenade.GetComponent<Rigidbody>().AddForce(swapThrowForce * GameManager.instance.playerCamera.transform.forward);
        }
        base.PickupCollectible(collectible, iconIndex, currentCollectible);
        collectible.GetComponent<SphereCollider>().enabled = false;
        GameManager.instance.currentGrenade = collectible;
        GameManager.instance.IconSwitchGrenade(iconIndex);
    }
}
