using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    GameObject transition;

    Animator transitionAnimator;

    [SerializeField]
    float transitionTime;

    AudioSource mainMenuBGM;

    float currentTimer = 0;

    bool startGame;

    private void Start()
    {
        GameManager.instance.playerHealth = GameManager.instance.setPlayerHealth;
        GameManager.instance.currentEquippable = null;
        GameManager.instance.currentGrenade = null;
        GameManager.instance.currentPrimary = null;
        GameManager.instance.healCount = 0;
        GameManager.instance.ammoText.text = "";
        GameManager.instance.extractionTimerUI.text = "";

        if (GameManager.instance.currentPrimaryIcon!= null)
        {
            GameManager.instance.currentPrimaryIcon.SetActive(false);
            GameManager.instance.currentPrimaryIcon = null;
        }
        if (GameManager.instance.currentGrenadeIcon != null)
        {
            GameManager.instance.currentGrenadeIcon.SetActive(false);
            GameManager.instance.currentGrenadeIcon = null;
        }
        AudioListener.pause = false;
        startGame = false;
        mainMenuBGM = GameManager.instance.BGM[9];
        mainMenuBGM.Play();
        GameManager.instance.currentBGM = mainMenuBGM;
        transition = GameManager.instance.transition;
        transitionAnimator = GameManager.instance.transitionAnimator;
        if (!GameManager.instance.firstLoad)
        {
            transitionAnimator.SetTrigger("Start");
            
        }
        else
        {
            GameManager.instance.firstLoad = false;
        }
        currentTimer = 0;
        Debug.Log(startGame);
    }

    private void Update()
    {
        if(startGame)
        {
            if (currentTimer == 0)
            {
                transitionAnimator.SetTrigger("End");
                Debug.Log("bruh");
            }
            currentTimer += Time.deltaTime;
            if (currentTimer >= transitionTime)
            {
                mainMenuBGM.Stop();
                startGame = false;
                currentTimer = 0;
                SceneManager.LoadScene(6);
            }
        }
    }

    public void StartGame()
    {
        startGame = true;
    }

    public void Options()
    {

    }


    public void Credits()
    {

    }

    public void HowToPlay()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void Back()
    {

    }

}
