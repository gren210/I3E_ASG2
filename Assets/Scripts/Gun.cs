using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gun : Interactable
{
    public string interactText;

    public float fireCooldown;

    private float currentCooldown;

    public float damage;

    public float bulletRange;

    public bool isFullAuto;

    public AudioClip gunShot;

    public AudioSource gunAudioSource;

    public AudioClip hitSound;

    public AudioSource reloadSound;

    public bool isReloading = false;

    [SerializeField]
    GameObject gunAudio;

    [SerializeField]
    float swapThrowForce;

    [SerializeField]
    float shakeTimer;

    float shakeTimerStart;

    [SerializeField]
    float shakeIntensity;

    [SerializeField]
    float shakeFrequency;

    [SerializeField]
    GameObject gunMuzzle;

    [SerializeField]
    ParticleSystem muzzleFlash;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    float bulletSpeed;

    public int ammoCount;

    [SerializeField]
    float reloadTime;

    float currentReloadTimer;

    [HideInInspector]
    public bool reloading;

    [HideInInspector]
    public int currentAmmoCount;

    public int iconIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = fireCooldown;
        currentAmmoCount = ammoCount;
        currentReloadTimer = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;

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

    public void Reload(Player thePlayer)
    {
        if (currentAmmoCount < ammoCount && !reloading)
        {
            reloadSound.Play();
            reloading = true;
        }
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        PickupCollectible(gameObject, iconIndex, GameManager.instance.currentPrimary);

    }

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
