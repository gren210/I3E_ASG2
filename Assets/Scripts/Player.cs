using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance;

    Interactable currentInteractable;

    Collectible currentCollectible;

    GiftBox currentGiftBox;

    Gun currentGunPickup;

    Grenade currentGrenadePickup;

    public GameObject currentGun;

    [SerializeField]
    GameObject equippedGun;

    [SerializeField]
    GameObject equippedGrenade;

    [SerializeField]
    float grenadeDistance;

    public TextMeshProUGUI interactionText;

    public TextMeshProUGUI jumpText;

    [SerializeField]
    Transform playerCamera;

    [SerializeField]
    float interactionDistance;

    private void Update()
    {
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, interactionDistance))
        {
            //Debug.Log(hitInfo.transform.name);

            hitInfo.transform.TryGetComponent<Collectible>(out currentCollectible);
            hitInfo.transform.TryGetComponent<GiftBox>(out currentGiftBox);
            hitInfo.transform.TryGetComponent<Gun>(out currentGunPickup);
            hitInfo.transform.TryGetComponent<Grenade>(out currentGrenadePickup);

            if (hitInfo.transform.TryGetComponent<Interactable>(out currentInteractable))
            {
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                currentInteractable = null;
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            currentInteractable = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    void OnInteract()
    {
        Debug.Log(currentInteractable);
        if(currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }
        if (currentCollectible != null)
        {
            currentCollectible.Interact(this);
        }
        if (currentGiftBox != null)
        {
            currentGiftBox.Interact(this);
        }
        if (currentGunPickup != null)
        {
            currentGunPickup.Interact(this);
        }
        if (currentGrenadePickup != null)
        {
            currentGrenadePickup.Interact(this);
        }
    }

    void OnFire()
    {
        if(GameManager.instance.currentEquippable == currentGun)
        {
            currentGun.GetComponent<Gun>().Shoot();
        }
        else if (GameManager.instance.currentEquippable == GameManager.instance.currentGrenade)
        {
            GameObject newGrenade = Instantiate(equippedGrenade, GameManager.instance.currentGrenade.transform.position, GameManager.instance.currentGrenade.transform.rotation);
            newGrenade.GetComponent<Grenade>().thrown = true;
            newGrenade.GetComponent<Rigidbody>().AddForce(grenadeDistance * playerCamera.transform.forward);
            //GameManager.instance.currentGrenade.GetComponent<Grenade>().Shoot();
        }



    }



    //void OnAutoFire()
    //{
        //currentGun.GetComponent<Gun>().Shoot();
    //}

    void OnEquipGun()
    {
        if (!GameManager.instance.isPrimary)
        {
            Debug.Log("no");
        }
        else
        {
            if (GameManager.instance.currentEquippable != null)
            {
                GameManager.instance.currentEquippable.SetActive(false);
            }
            //GameManager.instance.currentPrimary.SetActive(true);
            GameManager.instance.currentEquippable = GameManager.instance.currentPrimary;
            GameManager.instance.currentEquippable.SetActive(true);
        }
        
    }

    void OnEquipGrenade()
    {
        if (GameManager.instance.currentEquippable != null)
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }
        //GameManager.instance.currentGrenade.SetActive(true);
        GameManager.instance.currentEquippable = GameManager.instance.currentGrenade;
        GameManager.instance.currentEquippable.SetActive(true);
    }

    void OnHolster()
    {
        if(GameManager.instance.currentEquippable != null)
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }
    }

}

