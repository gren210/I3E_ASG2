/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Handles grenade interactions.
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grenade : Interactable
{
    /// <summary>
    /// The distance the grenade is thrown.
    /// </summary>
    [SerializeField]
    float grenadeDistance;

    /// <summary>
    /// The delay before the grenade explodes.
    /// </summary>
    public float delay = 3f;

    /// <summary>
    /// Timer to track the grenade's delay.
    /// </summary>
    float timer;

    /// <summary>
    /// The particle system for the grenade explosion effect.
    /// </summary>
    public ParticleSystem effect;

    /// <summary>
    /// The radius where the grenade will affect entities.
    /// </summary>
    public float explodeRadius = 50f;

    /// <summary>
    /// The damage dealt by the grenade explosion.
    /// </summary>
    public float explodeDamage = 70f;

    /// <summary>
    /// Indicates whether the grenade has been thrown.
    /// </summary>
    [HideInInspector]
    public bool thrown = false;

    /// <summary>
    /// Indicates whether the grenade has been set up for throwing.
    /// </summary>
    bool set = true;

    /// <summary>
    /// Indicates whether the grenade has exploded.
    /// </summary>
    bool exploded = false;

    /// <summary>
    /// The sound played when the grenade explodes.
    /// </summary>
    [SerializeField]
    AudioClip grenadeSound;

    /// <summary>
    /// The duration of the camera shake effect.
    /// </summary>
    [SerializeField]
    float shakeTimer;

    /// <summary>
    /// The starting value of the shake timer.
    /// </summary>
    float shakeTimerStart;

    /// <summary>
    /// The intensity of the camera shake effect.
    /// </summary>
    [SerializeField]
    float shakeIntensity;

    /// <summary>
    /// The frequency of the camera shake effect.
    /// </summary>
    [SerializeField]
    float shakeFrequency;

    /// <summary>
    /// The force applied when swapping the grenade.
    /// </summary>
    [SerializeField]
    float swapThrowForce;

    /// <summary>
    /// The index for the grenade icon.
    /// </summary>
    public int iconIndex;

    /// <summary>
    /// Handles the grenade explosion effect, applying damage to nearby entities.
    /// </summary>
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
                    Destroy(gameObject, 2f);
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

    /// <summary>
    /// Handles interaction with the player by picking up the grenade.
    /// </summary>
    /// <param name="thePlayer">The player interacting with the grenade.</param>
    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        PickupCollectible(gameObject, iconIndex, GameManager.instance.currentGrenade);
    }

    /// <summary>
    /// Handles picking up and equipping the grenade.
    /// </summary>
    /// <param name="collectible">The grenade GameObject to pick up.</param>
    /// <param name="iconIndex">The index of the grenade icon.</param>
    /// <param name="currentCollectible">The currently equipped collectible.</param>
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
