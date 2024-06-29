using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{


    GameObject transition;

    Animator transitionAnimator;

    float currentTimer;

    [SerializeField]
    float transitionTime;

    bool goBack = false;

    bool transitionPlayed = false;

    bool changeScene = false;
    bool changedScene = false;

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
            GameManager.instance.isImmune = true;
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
        if (changeScene)
        {
            GameManager.instance.isImmune = true;
            if (!changedScene)
            {
                transitionAnimator.SetTrigger("End");
                GameManager.instance.playerObject.GetComponent<FirstPersonController>().enabled = false;
                GameManager.instance.playerObject.GetComponent<Rigidbody>().isKinematic = true;
                changedScene = true;
            }
            currentTimer += Time.deltaTime;
            if (currentTimer >= transitionTime)
            {
                changeScene = false;
                changedScene = false;
                currentTimer = 0;
                SceneManager.LoadScene(GameManager.instance.currentScene);
                GameManager.instance.PersistItems();
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

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        GameManager.instance.pauseMenu.SetActive(false);
        GameManager.instance.DisableInput();
        GameManager.instance.playerHealth = 100;
        GameManager.instance.healCount = GameManager.instance.savedHealCount;
        changeScene = true;

    }

    public void MainMenu()
    {
        goBack = true;
        Time.timeScale = 1f;
    }
}
