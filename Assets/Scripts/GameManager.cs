using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerObject;

    public TextMeshProUGUI healthText;

    public float playerHealth = 100;

    public int grenadeCount = 0;

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

    public GameObject UI;

    public TextMeshProUGUI interactionText;

    public TextMeshProUGUI jumpText;

    public bool isTutorial;

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

        if(SceneManager.GetActiveScene().buildIndex >= 1)
        {
            isTutorial = false;
        }

    }

    //public void IncreaseScore(int scoreToAdd)
    //{
        //currentScore += scoreToAdd;
        //scoreText.text = "Score: " + currentScore;
    //}
}
