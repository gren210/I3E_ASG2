using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    float swapThrowForce;

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
            if(Physics.Raycast(GameManager.instance.playerCamera.position, GameManager.instance.playerCamera.forward, out hitInfo, bulletRange))
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
        gameObject.transform.SetParent(GameManager.instance.playerCamera.transform, false);

        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        GameManager.instance.currentPrimary = gameObject;
        GameManager.instance.currentEquippable = gameObject;
        GameManager.instance.currentEquippable.SetActive(true);

        gameObject.transform.position = GameManager.instance.equipPosition.transform.position;
        gameObject.transform.eulerAngles = GameManager.instance.equipPosition.transform.eulerAngles;

    }

}
