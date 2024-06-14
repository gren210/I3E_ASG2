using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //[SerializeField]
    //TextMeshProUGUI scoreText;

    public TextMeshProUGUI healthText;

    private int currentScore = 0;

    public float playerHealth = 100;

    public float grenadeCount = 0;

    public GameObject currentGrenade = null;

    public GameObject currentPrimary = null;

    public bool isPrimary = false;

    public GameObject currentFlashlight;

    public GameObject currentEquippable = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //scoreText.text = "Score: 0";
    }

    private void Update()
    {
        healthText.text = "Health: " + playerHealth;

    }

    public void IncreaseScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        //scoreText.text = "Score: " + currentScore;
    }
}
