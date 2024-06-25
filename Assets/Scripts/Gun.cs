using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Gun : Interactable
{
    public float fireCooldown;

    private float currentCooldown;

    public float damage;

    public float bulletRange;

    public AudioClip gunShot;

    public AudioSource gunAudioSource;

    public AudioClip hitSound;

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

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = fireCooldown;
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
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Shoot(Player thePlayer)
    {
        RaycastHit hitInfo;
        if (currentCooldown <= 0f)
        {
            bool hit = Physics.Raycast(thePlayer.playerCamera.position, thePlayer.playerCamera.forward, out hitInfo, bulletRange);
            gunAudioSource.PlayOneShot(gunShot);
            GameObject currentAudio = Instantiate(gunAudio);
            Destroy(currentAudio,2f);
            muzzleFlash.GetComponent<ParticleSystem>().Play();
            currentCooldown = fireCooldown;

            ShakeCamera(shakeIntensity, shakeFrequency);
            shakeTimerStart = shakeTimer;
            //GameObject newBullet = Instantiate(bullet, gunMuzzle.transform.position, gunMuzzle.transform.rotation);
            //newBullet.GetComponent<Rigidbody>().velocity = ((hitInfo.point - gunMuzzle.transform.position) * bulletSpeed);
            if (hit)
            {
                if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    Debug.Log("Enemy is shot");
                    enemy.enemyHealth -= damage;
                    AudioSource.PlayClipAtPoint(hitSound, hitInfo.point);
                }
                else if (hitInfo.transform.TryGetComponent<Boss>(out Boss boss))
                {
                    Debug.Log("Boss is shot");
                    boss.enemyHealth -= damage;
                    AudioSource.PlayClipAtPoint(hitSound, hitInfo.point);
                }
            }
        }
    }

    public override void Interact(Player thePlayer)
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
        if(GameManager.instance.currentEquippable != null && GameManager.instance.currentEquippable != GameManager.instance.currentPrimary)
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }
        gameObject.transform.SetParent(thePlayer.playerCamera.transform, false);

        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        GameManager.instance.currentPrimary = gameObject;
        GameManager.instance.currentEquippable = gameObject;
        GameManager.instance.currentEquippable.SetActive(true);

        gameObject.transform.position = GameManager.instance.equipPosition.transform.position;
        gameObject.transform.eulerAngles = GameManager.instance.equipPosition.transform.eulerAngles;

    }

}
