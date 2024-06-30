/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Handles the behavior and interaction of the Gun object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : Interactable
{
    /// <summary>
    /// Text displayed when interacting with the gun.
    /// </summary>
    public string interactText;

    /// <summary>
    /// The cooldown time between successive shots.
    /// </summary>
    public float fireCooldown;

    /// <summary>
    /// The current cooldown time remaining.
    /// </summary>
    private float currentCooldown;

    /// <summary>
    /// The amount of damage dealt by the gun.
    /// </summary>
    public float damage;

    /// <summary>
    /// The maximum range of the gun's bullets.
    /// </summary>
    public float bulletRange;

    /// <summary>
    /// Indicates if the gun is set to full auto mode.
    /// </summary>
    public bool isFullAuto;

    /// <summary>
    /// The sound played when the gun is fired.
    /// </summary>
    public AudioClip gunShot;

    /// <summary>
    /// The audio source used to play gunshot sounds.
    /// </summary>
    public AudioSource gunAudioSource;

    /// <summary>
    /// The sound played when the gun hits an object.
    /// </summary>
    public AudioClip hitSound;

    /// <summary>
    /// The audio source used to play reload sounds.
    /// </summary>
    public AudioSource reloadSound;

    /// <summary>
    /// Indicates if the gun is currently reloading.
    /// </summary>
    public bool isReloading = false;

    /// <summary>
    /// The game object used to handle gunshot audio.
    /// </summary>
    [SerializeField]
    GameObject gunAudio;

    /// <summary>
    /// The force applied when the gun is thrown or swapped.
    /// </summary>
    [SerializeField]
    float swapThrowForce;

    /// <summary>
    /// The duration of the camera shake effect when the gun is fired.
    /// </summary>
    [SerializeField]
    float shakeTimer;

    /// <summary>
    /// Timer to track the duration of the camera shake effect.
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
    /// The game object representing the gun's muzzle.
    /// </summary>
    [SerializeField]
    GameObject gunMuzzle;

    /// <summary>
    /// The particle system for the gun's muzzle flash.
    /// </summary>
    [SerializeField]
    ParticleSystem muzzleFlash;

    /// <summary>
    /// The prefab for the bullets fired by the gun.
    /// </summary>
    [SerializeField]
    GameObject bullet;

    /// <summary>
    /// The speed at which bullets are fired.
    /// </summary>
    [SerializeField]
    float bulletSpeed;

    /// <summary>
    /// The total amount of ammo available for the gun.
    /// </summary>
    public int ammoCount;

    /// <summary>
    /// The time required to reload the gun.
    /// </summary>
    [SerializeField]
    float reloadTime;

    /// <summary>
    /// Timer to track the remaining reload time.
    /// </summary>
    float currentReloadTimer;

    /// <summary>
    /// Indicates if the gun is currently reloading.
    /// </summary>
    [HideInInspector]
    public bool reloading;

    /// <summary>
    /// The current amount of ammo in the gun.
    /// </summary>
    [HideInInspector]
    public int currentAmmoCount;

    /// <summary>
    /// The index of the gun icon in the inventory.
    /// </summary>
    public int iconIndex;

    /// <summary>
    /// Initializes the gun with default values.
    /// </summary>
    void Start()
    {
        currentCooldown = fireCooldown;
        currentAmmoCount = ammoCount;
        currentReloadTimer = reloadTime;
    }

    void Update()
    {
        currentCooldown -= Time.deltaTime;

        // resets the camera shake
        if (shakeTimerStart > 0)
        {
            GameManager.instance.readySwap = false;
            shakeTimerStart -= Time.deltaTime;
            if (shakeTimerStart <= 0f)
            {
                ShakeCamera(0f, 0f);
                GameManager.instance.readySwap = true;
            }
        }

        //Checks if the reload button is pressed
        if (reloading)
        {
            currentReloadTimer -= Time.deltaTime;
            if (currentReloadTimer <= 0f)
            {
                reloading = false;
                isReloading = false;
                currentReloadTimer = reloadTime;
                currentAmmoCount = ammoCount;
            }
        }
    }

    /// <summary>
    /// Handles shooting logic
    /// </summary>
    /// <param name="thePlayer">The player shooting the gun.</param>
    public void Shoot(Player thePlayer)
    {
        if (currentAmmoCount != 0 && !reloading)
        {
            RaycastHit hitInfo;
            if (currentCooldown <= 0f)
            {
                bool hit = Physics.Raycast(thePlayer.playerCamera.position, thePlayer.playerCamera.forward, out hitInfo, bulletRange);
                GameObject currentAudio = Instantiate(gunAudio);
                Destroy(currentAudio, 1f);
                muzzleFlash.GetComponent<ParticleSystem>().Play();
                currentCooldown = fireCooldown;
                currentAmmoCount--;

                ShakeCamera(shakeIntensity, shakeFrequency);
                shakeTimerStart = shakeTimer;

                if (hit)
                {
                    if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        UpdateEnemyHealth(hitInfo.transform.gameObject, damage);
                        AudioSource.PlayClipAtPoint(hitSound, hitInfo.point);
                    }
                    else if (hitInfo.transform.TryGetComponent<Boss>(out Boss boss))
                    {
                        UpdateEnemyHealth(hitInfo.transform.gameObject, damage);
                        AudioSource.PlayClipAtPoint(hitSound, hitInfo.point);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Initiates the reload process for the gun.
    /// </summary>
    /// <param name="thePlayer">The player reloading the gun.</param>
    public void Reload(Player thePlayer)
    {
        if (currentAmmoCount < ammoCount && !reloading)
        {
            reloadSound.Play();
            reloading = true;
        }
    }

    /// <summary>
    /// Handles interaction with the gun, including equipping it.
    /// </summary>
    /// <param name="thePlayer">The player interacting with the gun.</param>
    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        PickupCollectible(gameObject, iconIndex, GameManager.instance.currentPrimary);
    }

    /// <summary>
    /// Handles picking up the gun and updating the player's inventory. Derived from the Interactable classs.
    /// </summary>
    /// <param name="collectible">The gun object being picked up.</param>
    /// <param name="iconIndex">The index of the gun icon.</param>
    /// <param name="currentCollectible">The current collectible in the player's inventory.</param>
    protected override void PickupCollectible(GameObject collectible, int iconIndex, GameObject currentCollectible)
    {
        if (GameManager.instance.currentPrimary != null)
        {
            if (GameManager.instance.currentPrimary.activeSelf == false)
            {
                GameManager.instance.currentPrimary.SetActive(true);
            }
            GameManager.instance.currentPrimary.transform.parent = null;
            GameManager.instance.currentPrimary.GetComponent<BoxCollider>().enabled = true;
            GameManager.instance.currentPrimary.GetComponent<Rigidbody>().isKinematic = false;
            GameManager.instance.currentPrimary.GetComponent<Rigidbody>().AddForce(swapThrowForce * GameManager.instance.playerCamera.transform.forward);
        }
        base.PickupCollectible(collectible, iconIndex, currentCollectible);
        collectible.GetComponent<BoxCollider>().enabled = false;
        GameManager.instance.currentPrimary = collectible;
        GameManager.instance.IconSwitchPrimary(iconIndex);
    }
}

