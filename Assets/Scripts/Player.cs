/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Script for controlling the player's behavior, including interaction with objects, shooting, reloading, and managing the flashlight.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class Player : ScriptManager
 {
    /// <summary>
    /// Stores the current interactables being looked at with raycast.
    /// </summary>
    Interactable currentInteractable;

    /// <summary>
    /// Stores the current gun looked at.
    /// </summary>
    Gun currentGunPickup;

    /// <summary>
    /// Stores the current grenade looked at.
    /// </summary>
    Grenade currentGrenadePickup;

    /// <summary>
    /// Stores the current crystal looked at.
    /// </summary>
    Crystal currentCrystal;

    /// <summary>
    /// Stores the current door looked at.
    /// </summary>
    Door currentDoor;

    /// <summary>
    /// Stores the current key looked at.
    /// </summary>
    Key currentKey;

    /// <summary>
    /// Stores the current lever looked at.
    /// </summary>
    Lever currentLever;

    /// <summary>
    /// Stores the current medkit looked at.
    /// </summary>
    Heal currentHeal;

    /// <summary>
    /// Stores the current reactor looked at.
    /// </summary>
    PlaceCrystal currentPlaceCrystal;

    /// <summary>
    /// Stores the player's flashlight GameObject.
    /// </summary>
    [SerializeField]
    GameObject flashlight;

    /// <summary>
    /// The player's controller GameObject.
    /// </summary>
    public GameObject playerController;

    /// <summary>
    /// The UI text for interaction prompts.
    /// </summary>
    public TextMeshProUGUI interactionText;

    /// <summary>
    /// The UI text for jump prompts.
    /// </summary>
    public TextMeshProUGUI jumpText;

    /// <summary>
    /// The player's camera transform.
    /// </summary>
    public Transform playerCamera;

    /// <summary>
    /// The distance where the player can interact with objects.
    /// </summary>
    [SerializeField]
    float interactionDistance;

    /// <summary>
    /// The Cinemachine virtual camera of the player.
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// The position where equipped items are held.
    /// </summary>
    [SerializeField]
    GameObject equipPosition;

    /// <summary>
    /// Bool to check if the player is in the spaceship in the first level.
    /// </summary>
    [SerializeField]
    bool inSpaceship;

    /// <summary>
    /// This start function initialises all its variables after loading a new scene.
    /// </summary>
    private void Start()
    {
        GameManager.instance.playerObject = gameObject;
        GameManager.instance.playerCamera = playerCamera;
        GameManager.instance.virtualCamera = virtualCamera;
        GameManager.instance.equipPosition = equipPosition;
        if (GameManager.instance.currentPrimary != null)
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
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, interactionDistance))
        {
            hitInfo.transform.TryGetComponent<Gun>(out currentGunPickup);
            hitInfo.transform.TryGetComponent<Grenade>(out currentGrenadePickup);
            hitInfo.transform.TryGetComponent<Crystal>(out currentCrystal);
            hitInfo.transform.TryGetComponent<Door>(out currentDoor);
            hitInfo.transform.TryGetComponent<Key>(out currentKey);
            hitInfo.transform.TryGetComponent<Lever>(out currentLever);
            hitInfo.transform.TryGetComponent<Heal>(out currentHeal);
            hitInfo.transform.TryGetComponent<PlaceCrystal>(out currentPlaceCrystal);

            if (hitInfo.transform.TryGetComponent<Interactable>(out currentInteractable))
            {
                GameManager.instance.interactionBox.SetActive(true);
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                GameManager.instance.interactionBox.SetActive(false);
                currentInteractable = null;
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            GameManager.instance.interactionBox.SetActive(false);
            currentInteractable = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handles interaction with all the interactable game objects.
    /// </summary>
    void OnInteract()
    {
        if (currentGunPickup != null)
        {
            currentGunPickup.Interact(this);
        }
        if (currentGrenadePickup != null && !currentGrenadePickup.thrown)
        {
            currentGrenadePickup.Interact(this);
        }
        if (currentDoor != null)
        {
            currentDoor.Interact(this);
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
        if (currentHeal != null)
        {
            currentHeal.Interact(this);
        }
        if (currentPlaceCrystal != null)
        {
            currentPlaceCrystal.Interact(this);
        }
    }

    /// <summary>
    /// Handles pausing the game.
    /// </summary>
    void OnPause()
    {
        CursorLock(false);
        if (!GameManager.instance.pauseMenu.activeSelf)
        {
            AudioListener.pause = true;
            LockInput(true);
            GameManager.instance.pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// Handles firing the primary weapon or throwing the grenade.
    /// </summary>
    void OnFire()
    {
        if (!inSpaceship && GameManager.instance.currentEquippable != null)
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

    /// <summary>
    /// Handles reloading the primary weapon.
    /// </summary>
    void OnReload()
    {
        if (GameManager.instance.currentEquippable == GameManager.instance.currentPrimary)
        {
            GameManager.instance.currentPrimary.GetComponent<Gun>().Reload(this);
        }
    }

    /// <summary>
    /// Handles equipping the primary gun.
    /// </summary>
    void OnEquipGun()
    {
        if (GameManager.instance.currentEquippable != GameManager.instance.currentPrimary && GameManager.instance.currentPrimary != null)
        {
            EquipCollectible(GameManager.instance.currentPrimary);
        }
    }

    /// <summary>
    /// Handles equipping the grenade.
    /// </summary>
    void OnEquipGrenade()
    {
        if (GameManager.instance.readySwap && !GameManager.instance.currentPrimary.GetComponent<Gun>().reloading && GameManager.instance.currentGrenade != null && GameManager.instance.currentEquippable != GameManager.instance.currentGrenade)
        {
            EquipCollectible(GameManager.instance.currentGrenade);
        }
    }

    /// <summary>
    /// Equips a collectible item, meant for the primary and grenade.
    /// </summary>
    /// <param name="currentCollectible">The collectible item to equip.</param>
    void EquipCollectible(GameObject currentCollectible)
    {
        if (GameManager.instance.currentEquippable != null)
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }
        GameManager.instance.currentEquippable = currentCollectible;
        GameManager.instance.currentEquippable.SetActive(true);
        GameManager.instance.swapItemSound.Play();
    }

    /// <summary>
    /// Toggles the player's flashlight on and off.
    /// </summary>
    void OnFlashlight()
    {
        flashlight.SetActive(!flashlight.activeSelf);
        GameManager.instance.flashlightSound.Play();
    }

    /// <summary>
    /// Handles using a medkit.
    /// </summary>
    void OnHeal()
    {
        if (GameManager.instance.healCount > 0 && GameManager.instance.playerHealth != 100)
        {
            GameManager.instance.healCount--;
            GameManager.instance.playerHealth += GameManager.instance.healAmount;
            if (GameManager.instance.playerHealth > 100)
            {
                GameManager.instance.playerHealth = 100;
            }
            GameManager.instance.UpdateHealth();
            GameManager.instance.healSound.Play();
        }
    }
}

