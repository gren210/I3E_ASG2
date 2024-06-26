using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerObject;

    public TextMeshProUGUI healthText;

    public float playerHealth = 100;

    public Image healthBar;

    public GameObject equipPosition;

    public GameObject currentFlashlight;

    public GameObject currentEquippable = null;

    public Transform playerCamera;

    public CinemachineVirtualCamera virtualCamera;

    //public GameObject equipParent;

    public bool isPrimary = false;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(UI);

        }
        else if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //scoreText.text = "Score: 0";

        Application.targetFrameRate = 240;
    }

    private void Update()
    {
        healthText.text = "Health: " + playerHealth;
        healText.text = "" + healCount;
        if (currentEquippable == currentPrimary && currentEquippable != null)
        {
            ammoText.text = "" + currentEquippable.GetComponent<Gun>().currentAmmoCount;
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

    //public void IncreaseScore(int scoreToAdd)
    //{
    //currentScore += scoreToAdd;
    //scoreText.text = "Score: " + currentScore;
    //}
}
