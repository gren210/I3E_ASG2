/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: The Game Manager which handles important game information and persists through scenes.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using StarterAssets;
using UnityEngine.InputSystem;

public class GameManager : ScriptManager
{
    /// <summary>
    /// An instance of the GameManager so that all scripts can access the GameManager.
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// GameObject holding the player gameObject.
    /// </summary>
    public GameObject playerObject;

    /// <summary>
    /// float that stores the Player's current health.
    /// </summary>
    [HideInInspector]
    public float playerHealth = 100;

    /// <summary>
    /// Float that stores the default player health.
    /// </summary>
    public float setPlayerHealth = 100;

    /// <summary>
    /// Image component for the health bar UI.
    /// </summary>
    public Image healthBar;

    /// <summary>
    /// GameObject which is the position where equipment is held.
    /// </summary>
    public GameObject equipPosition;

    /// <summary>
    /// The current equipped item.
    /// </summary>
    [HideInInspector]
    public GameObject currentEquippable = null;

    /// <summary>
    /// GameObject which holds the player's camera transform.
    /// </summary>
    [HideInInspector]
    public Transform playerCamera;

    /// <summary>
    /// The virtual camera for the player.
    /// </summary>
    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// Bool to check if the player is ready to swap items.
    /// </summary>
    [HideInInspector]
    public bool readySwap = true;

    /// <summary>
    /// GameObject which holds the current grenade held by the player.
    /// </summary>
    [HideInInspector]
    public GameObject currentGrenade = null;

    /// <summary>
    /// GameObject which holds the current primary weapon held by the player.
    /// </summary>
    [HideInInspector]
    public GameObject currentPrimary = null;

    /// <summary>
    /// Bool to check if the player has the crystal.
    /// </summary>
    [HideInInspector]
    public bool haveCrystal;

    /// <summary>
    /// gameObject for scene transitions.
    /// </summary>
    public GameObject transition;

    /// <summary>
    /// Animator for handling transition animations.
    /// </summary>
    public Animator transitionAnimator;

    /// <summary>
    /// Int which stores the current number of heals the player has.
    /// </summary>
    [HideInInspector]
    public int healCount;

    /// <summary>
    /// Int which stores the number of heals saved at the last checkpoint.
    /// </summary>
    [HideInInspector]
    public int savedHealCount;

    /// <summary>
    /// Int which stores the amount of health restored per heal.
    /// </summary>
    public int healAmount;

    /// <summary>
    /// GameObject which stores the parent object for all game UI elements.
    /// </summary>
    public GameObject allGameUI;

    /// <summary>
    /// GameObject which stores the UI element for the main game interface.
    /// </summary>
    public GameObject UI;

    /// <summary>
    /// GameObject which stores the UI element for displaying the interaction box.
    /// </summary>
    public GameObject interactionBox;

    /// <summary>
    /// Text component for interactions.
    /// </summary>
    public TextMeshProUGUI interactionText;

    /// <summary>
    /// Text component for jump text.
    /// </summary>
    public TextMeshProUGUI jumpText;

    /// <summary>
    /// Text component for ammo count.
    /// </summary>
    public TextMeshProUGUI ammoText;

    /// <summary>
    /// Text component for heal count.
    /// </summary>
    public TextMeshProUGUI healText;

    /// <summary>
    /// Text component for displaying the current game objectives.
    /// </summary>
    public TextMeshProUGUI objectiveText;

    /// <summary>
    /// Array of GameObject which stores the primary weapon icons.
    /// </summary>
    [SerializeField]
    GameObject[] primaryIcons;

    /// <summary>
    /// GameObject which stores the current icon representing the primary weapon.
    /// </summary>
    [HideInInspector]
    public GameObject currentPrimaryIcon;

    /// <summary>
    /// Array of GameObjects which stores the  grenade icons.
    /// </summary>
    [SerializeField]
    GameObject[] grenadeIcons;

    /// <summary>
    /// GameObject which stores the current icon representing the grenade.
    /// </summary>
    [HideInInspector]
    public GameObject currentGrenadeIcon;

    /// <summary>
    /// Array of background music audio sources.
    /// </summary>
    public AudioSource[] BGM;

    /// <summary>
    /// Array of the objective description text.
    /// </summary>
    public string[] objectiveStrings;

    /// <summary>
    /// Stores the background music currently playing.
    /// </summary>
    [HideInInspector]
    public AudioSource currentBGM;

    /// <summary>
    /// Audio source which stores the item swap sound effect.
    /// </summary>
    public AudioSource swapItemSound;

    /// <summary>
    /// Audio source which stores the flashlight sound effect.
    /// </summary>
    public AudioSource flashlightSound;

    /// <summary>
    /// Audio source which stores the grenade throw sound effect.
    /// </summary>
    public AudioSource throwSound;

    /// <summary>
    /// Audio source which stores the heal sound effect.
    /// </summary>
    public AudioSource healSound;

    /// <summary>
    /// GameObject which stores the UI element for the pause menu.
    /// </summary>
    public GameObject pauseMenu;

    /// <summary>
    /// GameObject which stores the parent object for all pause menu UI elements.
    /// </summary>
    public GameObject allPauseUI;

    /// <summary>
    /// GameObject which stores the UI element for the game over screen.
    /// </summary>
    public GameObject gameOverUI;

    /// <summary>
    /// Bool to check if this is the first time loading the game.
    /// </summary>
    [HideInInspector]
    public bool firstLoad = true;

    /// <summary>
    /// Bool to check if the player has died.
    /// </summary>
    bool hasDied = false;

    /// <summary>
    /// Text component for the extraction timer.
    /// </summary>
    public TextMeshProUGUI extractionTimerUI;

    /// <summary>
    /// Int which stores the current scene index.
    /// </summary>
    [HideInInspector]
    public int currentScene;

    /// <summary>
    /// Bool to check if the game has restarted.
    /// </summary>
    [HideInInspector]
    public bool hasRestarted = false;

    /// <summary>
    /// Bool to check if the player is immune to damage.
    /// </summary>
    [HideInInspector]
    public bool isImmune;

    /// <summary>
    /// This Awake function ensures that there is only one instance of the GameManager.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(allGameUI);
            DontDestroyOnLoad(allPauseUI);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Destroy(allGameUI);
            Destroy(allPauseUI);
        }
    }

    /// <summary>
    /// This update function constantly updates the medkit count, ammo, and checks if the player has died.
    /// </summary>
    private void Update()
    {
        
        healText.text = "" + healCount;
        if (currentEquippable == currentPrimary && currentEquippable != null)
        {
            ammoText.text = "" + currentEquippable.GetComponent<Gun>().currentAmmoCount;
        }
        if (playerHealth <= 0 && !hasDied)
        {
            hasDied = true;
            AudioListener.pause = true;
            CursorLock(false);
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            LockInput(true);
        }
    }

    /// <summary>
    /// Updates the player's health bar UI.
    /// </summary>
    public void UpdateHealth()
    {
        healthBar.fillAmount = playerHealth / 100f;
    }

    /// <summary>
    /// Switches the primary weapon icon.
    /// </summary>
    /// <param name="index">Index of the primary weapon icon to switch to.</param>
    public void IconSwitchPrimary(int index)
    {
        if (currentPrimaryIcon != primaryIcons[index])
        {
            if (currentPrimaryIcon != null)
            {
                currentPrimaryIcon.SetActive(false);
            }
            currentPrimaryIcon = primaryIcons[index];
            currentPrimaryIcon.SetActive(true);
        }
    }

    /// <summary>
    /// Switches the grenade icon.
    /// </summary>
    /// <param name="index">Index of the grenade icon to switch to.</param>
    public void IconSwitchGrenade(int index)
    {
        if (currentGrenadeIcon != grenadeIcons[index])
        {
            if (currentGrenadeIcon != null)
            {
                currentGrenadeIcon.SetActive(false);
            }
            currentGrenadeIcon = grenadeIcons[index];
            currentGrenadeIcon.SetActive(true);
        }
    }

    /// <summary>
    /// Persists the items through scenes.
    /// </summary>
    public void PersistItems()
    {
        if (currentPrimary != null)
        {
            currentPrimary.transform.SetParent(gameObject.transform, false);
        }
        if (currentGrenade != null)
        {
            currentGrenade.transform.SetParent(gameObject.transform, false);
        }
    }
}


