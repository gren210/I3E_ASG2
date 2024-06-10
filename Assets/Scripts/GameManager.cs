using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //[SerializeField]
    //TextMeshProUGUI scoreText;

    private int currentScore = 0;

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

    public void IncreaseScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        //scoreText.text = "Score: " + currentScore;
    }
}
