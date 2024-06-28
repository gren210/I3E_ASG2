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

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerObject;

    [HideInInspector]
    public float playerHealth = 100;

    public float setPlayerHealth = 100;

    public Image healthBar;

    public GameObject equipPosition;

    //public GameObject currentFlashlight;

    [HideInInspector]
    public GameObject currentEquippable = null;

    [HideInInspector]
    public Transform playerCamera;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    //public GameObject equipParent;

    //public bool isPrimary = false;

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

    public int healCount;

    public int healAmount;

    public GameObject allGameUI;

    public GameObject UI;

    public GameObject interactionBox;

    public TextMeshProUGUI interactionText;

    public TextMeshProUGUI jumpText;

    public TextMeshProUGUI ammoText;

    public TextMeshProUGUI healText;

    [SerializeField]
    GameObject[] primaryIcons;

    GameObject currentPrimaryIcon;

    [SerializeField]
    GameObject[] grenadeIcons;

    [HideInInspector]
    public GameObject currentGrenadeIcon;

    public AudioSource[] BGM;

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
        //scoreText.text = "Score: 0";

        Application.targetFrameRate = 240;
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            DisableInput();
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

    public void DisableInput()
    {
        playerObject.GetComponent<FirstPersonController>().enabled = false;
        playerObject.GetComponent<PlayerInput>().enabled = false;
        playerObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void EnableInput()
    {

        playerObject.GetComponent<FirstPersonController>().enabled = true;
        playerObject.GetComponent<PlayerInput>().enabled = true;
        playerObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    //public void IncreaseScore(int scoreToAdd)
    //{
    //currentScore += scoreToAdd;
    //scoreText.text = "Score: " + currentScore;
    //}
}
