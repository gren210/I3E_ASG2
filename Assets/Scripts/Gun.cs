using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Interactable
{
    public float fireCooldown;

    private float currentCooldown;

    public float damage;

    public float bulletRange;

    public AudioClip gunShot;

    public AudioSource gunAudioSource;

    [SerializeField]
    Transform playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = fireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        RaycastHit hitInfo;
        if (currentCooldown <= 0f)
        {
            gunAudioSource.PlayOneShot(gunShot);
            currentCooldown = fireCooldown;
            if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, bulletRange))
            {
                if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    Debug.Log("Gun is shot");
                    enemy.enemyHealth -= damage;
                }
            }
        }


    }

    public override void Interact(Player thePlayer)
    {
        if (!GameManager.instance.isPrimary)
        {
            GameManager.instance.isPrimary = true;
        }
        if (GameManager.instance.currentEquippable != null) 
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }
        GameManager.instance.currentEquippable = GameManager.instance.currentPrimary;
        GameManager.instance.currentEquippable.SetActive(true);
        Destroy(gameObject);
    }

}
