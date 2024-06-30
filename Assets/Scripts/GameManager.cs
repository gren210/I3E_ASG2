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
    public static GameManager instance;

    public GameObject playerObject;

    [HideInInspector]
    public float playerHealth = 100;

    public float setPlayerHealth = 100;

    public Image healthBar;

    public GameObject equipPosition;

    [HideInInspector]
    public GameObject currentEquippable = null;

    [HideInInspector]
    public Transform playerCamera;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    [HideInInspector]
    public bool readySwap = true;

    [HideInInspector]
    public GameObject currentGrenade = null;

    [HideInInspector]
    public GameObject currentPrimary = null;

    [HideInInspector]
    public bool haveCrystal;

    public GameObject transition;

    public Animator transitionAnimator;

    [HideInInspector]
    public int healCount;

    [HideInInspector]
    public int savedHealCount;

    public int healAmount;

    public GameObject allGameUI;

    public GameObject UI;

    public GameObject interactionBox;

    public TextMeshProUGUI interactionText;

    public TextMeshProUGUI jumpText;

    public TextMeshProUGUI ammoText;

    public TextMeshProUGUI healText;

    public TextMeshProUGUI objectiveText;

    [SerializeField]
    GameObject[] primaryIcons;

    [HideInInspector]
    public GameObject currentPrimaryIcon;

    [SerializeField]
    GameObject[] grenadeIcons;

    [HideInInspector]
    public GameObject currentGrenadeIcon;

    public AudioSource[] BGM;

    public string[] objectiveStrings;

    [HideInInspector]
    public AudioSource currentBGM;

    public AudioSource swapItemSound;

    public AudioSource flashlightSound;

    public AudioSource throwSound;

    public AudioSource healSound;

    public GameObject pauseMenu;

    public GameObject allPauseUI;

    public GameObject gameOverUI;

    [HideInInspector]
    public bool firstLoad = true;

    bool hasDied = false;

    public TextMeshProUGUI extractionTimerUI;

    [HideInInspector]
    public GameObject currentCheckpoint;

    [HideInInspector]
    public bool hasRestartedCheckpoint;

    float spawnTimer = 0;

    [HideInInspector]
    public int currentScene;

    [HideInInspector]
    public bool hasRestarted = false;

    [HideInInspector]
    public bool isImmune;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(allGameUI);
            DontDestroyOnLoad(allPauseUI);

        }
        else if(instance != null && instance != this)
        {
            Destroy(gameObject);
            Destroy(allGameUI);
            Destroy(allPauseUI);
        }
    }

    private void Start()
    {

    }

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

    public void UpdateHealth()
    {
        healthBar.fillAmount = playerHealth / 100f;
    }

    public void IconSwitchPrimary(int index)
    {
        if(currentPrimaryIcon != primaryIcons[index])
        {
            if(currentPrimaryIcon != null)
            {
                currentPrimaryIcon.SetActive(false);
            }
            currentPrimaryIcon = primaryIcons[index];
            currentPrimaryIcon.SetActive(true);
        }
    }

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
