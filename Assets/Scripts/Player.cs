using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Player : MonoBehaviour
{
    public static Player instance;

    Interactable currentInteractable;

    GiftBox currentGiftBox;

    Gun currentGunPickup;

    Grenade currentGrenadePickup;

    Crystal currentCrystal;

    Door currentDoor;

    Key currentKey;

    Lever currentLever;

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

    [SerializeField]
    CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    GameObject equipPosition;

    private GameObject playerObject;

    [SerializeField]
    bool inSpaceship;

    private void Awake()
    {
        //playerObject = gameObject;    
    }

    private void Start()
    {
        GameManager.instance.playerObject = gameObject;
        GameManager.instance.playerCamera = playerCamera;
        GameManager.instance.virtualCamera = virtualCamera;
        GameManager.instance.equipPosition = equipPosition;
        if (GameManager.instance.currentPrimary != null )
        {
            GameManager.instance.currentPrimary.transform.SetParent(playerCamera, false);
        }
        if (GameManager.instance.currentGrenade != null)
        {
            GameManager.instance.currentGrenade.transform.SetParent(playerCamera, false);
        }
        interactionText = GameManager.instance.interactionText;
        jumpText = GameManager.instance.jumpText;



    }

    private void Update()
    {
        //Debug.Log(GameManager.instance.playerObject);
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, interactionDistance))
        {
            //Debug.Log(hitInfo.transform.name);
            hitInfo.transform.TryGetComponent<GiftBox>(out currentGiftBox);
            hitInfo.transform.TryGetComponent<Gun>(out currentGunPickup);
            hitInfo.transform.TryGetComponent<Grenade>(out currentGrenadePickup);
            hitInfo.transform.TryGetComponent<Crystal>(out currentCrystal);
            hitInfo.transform.TryGetComponent<Door>(out currentDoor);
            hitInfo.transform.TryGetComponent<Key>(out currentKey);
            hitInfo.transform.TryGetComponent<Lever>(out currentLever);

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
        if (currentCrystal != null)
        {
            currentCrystal.Interact(this);
        }
        if (currentKey != null)
        {
            currentKey.Interact(this);
        }
        if (currentLever != null)
        {
            currentLever.Interact(this);
        }

    }

    void OnFire()
    {
        if (!inSpaceship)
        {
            if (GameManager.instance.currentEquippable != null)
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

    void OnHeal()
    {
        if (GameManager.instance.healCount > 0)
        {
            GameManager.instance.healCount--;
            GameManager.instance.playerHealth += GameManager.instance.healAmount;

        }
    }

    //void OnAutoFire()
    //{
        //currentGun.GetComponent<Gun>().Shoot();
    //}

    

}

