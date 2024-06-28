using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{


    GameObject transition;

    Animator transitionAnimator;

    float currentTimer;

    [SerializeField]
    float transitionTime;

    bool goBack = false;

    bool transitionPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.pauseMenu.SetActive(false);
        transition = GameManager.instance.transition;
        transitionAnimator = GameManager.instance.transitionAnimator;
        currentTimer = 0;
    }

    private void Update()
    {
        if (goBack)
        {
            if (currentTimer == 0 && !transitionPlayed)
            {
                transitionAnimator.SetTrigger("End");
                transitionPlayed = true;
            }
            currentTimer += Time.deltaTime;
            if (currentTimer >= transitionTime)
            {
                //mainMenuBGM.Stop();
                GameManager.instance.pauseMenu.SetActive(false);
                goBack = false;
                currentTimer = 0;
                transitionPlayed = false;
                GameManager.instance.UI.SetActive(false);
                GameManager.instance.currentBGM.Stop();
                SceneManager.LoadScene(0);

            }
        }
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.instance.EnableInput();
        GameManager.instance.pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Checkpoint()
    {

    }

    public void RestartLevel()
    {

    }

    public void MainMenu()
    {
        goBack = true;
        Time.timeScale = 1f;
    }
}
