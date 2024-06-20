using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance;

    Interactable currentInteractable;

    GiftBox currentGiftBox;

    Gun currentGunPickup;

    Grenade currentGrenadePickup;

    //public GameObject currentGun;

    //[SerializeField]
    //GameObject equippedGun;

    //[SerializeField]
    //GameObject equippedGrenade;

    [SerializeField]
    GameObject flashlight;

    public GameObject playerController;

    public TextMeshProUGUI interactionText;

    public TextMeshProUGUI jumpText;

    public Transform playerCamera;

    [SerializeField]
    float interactionDistance;

    private void Awake()
    {
        DontDestroyOnLoad(playerController);
    }
    private void Update()
    {
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, interactionDistance))
        {
            //Debug.Log(hitInfo.transform.name);
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
            if (currentGrenadePickup.thrown == false)
            {
                currentGrenadePickup.Interact(this);
            }
        }
    }

    void OnFire()
    {
        if(GameManager.instance.currentEquippable != null)
        {
            if (GameManager.instance.currentEquippable == GameManager.instance.currentPrimary)
            {
                GameManager.instance.currentPrimary.GetComponent<Gun>().Shoot(this);
            }
            else if (GameManager.instance.currentEquippable == GameManager.instance.currentGrenade)
            {
                GameManager.instance.currentEquippable.GetComponent<Grenade>().thrown = true;
            }
        }
    }

    void OnEquipGun()
    {
        if (GameManager.instance.readySwap == true && GameManager.instance.currentEquippable != null)
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }
        GameManager.instance.currentEquippable = GameManager.instance.currentPrimary;
        GameManager.instance.currentEquippable.SetActive(true);

    }

    void OnEquipGrenade()
    {
        if(GameManager.instance.readySwap == true && GameManager.instance.grenadeCount > 0)
        {
            if (GameManager.instance.currentEquippable != null)
            {
                GameManager.instance.currentEquippable.SetActive(false);
            }
            GameManager.instance.currentEquippable = GameManager.instance.currentGrenade;
            GameManager.instance.currentEquippable.SetActive(true);
        }
    }

    void OnFlashlight()
    {
        if (flashlight.activeSelf == false)
        {
            flashlight.SetActive(true);
        }
        else
        {
            flashlight.SetActive(false);
        }
    }


    //void OnAutoFire()
    //{
        //currentGun.GetComponent<Gun>().Shoot();
    //}

    

}

