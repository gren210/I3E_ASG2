using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    GameObject transition;

    Animator transitionAnimator;

    [SerializeField]
    float transitionTime;

    AudioSource mainMenuBGM;

    float currentTimer;

    bool startGame = false;

    private void Start()
    {
        mainMenuBGM = GameManager.instance.BGM[5];
        mainMenuBGM.Play();
        currentTimer = 0;
    }

    private void Update()
    {
        if(startGame)
        {
            if (currentTimer == 0)
            {
                transitionAnimator.SetTrigger("End");
            }
            currentTimer += Time.deltaTime;
            if (currentTimer >= transitionTime)
            {
                mainMenuBGM.Stop();
                SceneManager.LoadScene(0);
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
