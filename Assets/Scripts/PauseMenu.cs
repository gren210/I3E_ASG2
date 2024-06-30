/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Script for the pause menu.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEditor;

public class PauseMenu : ScriptManager
{
    /// <summary>
    /// The transition gameObject for scene transitions.
    /// </summary>
    GameObject transition;

    /// <summary>
    /// Animator for handling the transition animation.
    /// </summary>
    Animator transitionAnimator;

    /// <summary>
    /// Float timer to track transition time.
    /// </summary>
    float currentTimer;

    /// <summary>
    /// The duration of the transition animation.
    /// </summary>
    [SerializeField]
    float transitionTime;

    /// <summary>
    /// Bool to check if the player is going back to the main menu.
    /// </summary>
    bool goBack = false;

    /// <summary>
    /// Bool to check if the transition animation has played.
    /// </summary>
    bool transitionPlayed = false;

    /// <summary>
    /// Bool to check if the scene should change.
    /// </summary>
    bool changeScene = false;

    /// <summary>
    /// Bool to check if the scene transition is played.
    /// </summary>
    bool changedScene = false;

    void Start()
    {
        transition = GameManager.instance.transition;
        transitionAnimator = GameManager.instance.transitionAnimator;
        currentTimer = 0;
    }

    private void Update()
    {
        // These codes are for the animation transition.
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

    /// <summary>
    /// Resumes the game, activated by a UI button.
    /// </summary>
    public void ResumeGame()
    {
        AudioListener.pause = false;
        CursorLock(true);
        LockInput(false);
        GameManager.instance.pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Restarts the current level, activated by a UI button.
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        GameManager.instance.currentBGM.Stop();
        GameManager.instance.pauseMenu.SetActive(false);
        GameManager.instance.gameOverUI.SetActive(false);
        LockInput(true);
        GameManager.instance.playerHealth = 100;
        GameManager.instance.healCount = GameManager.instance.savedHealCount;
        changeScene = true;
    }

    /// <summary>
    /// Returns to the main menu, activated by a UI button.
    /// </summary>
    public void MainMenu()
    {
        goBack = true;
        Time.timeScale = 1f;
    }
}