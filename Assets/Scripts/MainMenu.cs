/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Script for the main menu functions.
 */

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MainMenu : ScriptManager
{
    /// <summary>
    /// The transition gameObject for scene transitions.
    /// </summary>
    GameObject transition;

    /// <summary>
    /// Animator for handling the transition animations.
    /// </summary>
    Animator transitionAnimator;

    /// <summary>
    /// The duration of the transition animation.
    /// </summary>
    [SerializeField]
    float transitionTime;

    /// <summary>
    /// AudioSource for the main menu background music.
    /// </summary>
    AudioSource mainMenuBGM;

    /// <summary>
    /// Timer to track the time in the Update function.
    /// </summary>
    float currentTimer = 0;

    /// <summary>
    /// Bool to check if the player presses start.
    /// </summary>
    bool startGame;

    /// <summary>
    /// Audio mixer for controlling volume levels.
    /// </summary>
    public AudioMixer volumeMixer;

    /// <summary>
    /// Slider for adjusting the music volume.
    /// </summary>
    [SerializeField]
    Slider musicVolumeSlider;

    // Start is called before the first frame update
    private void Start()
    {
        // This chunk of code is to initialise the game, resetting every value of the game.
        if (GameManager.instance.currentBGM != null)
        {
            GameManager.instance.currentBGM.Stop();
        }
        GameManager.instance.playerHealth = GameManager.instance.setPlayerHealth;
        GameManager.instance.currentEquippable = null;
        GameManager.instance.currentGrenade = null;
        GameManager.instance.currentPrimary = null;
        GameManager.instance.healCount = 0;
        GameManager.instance.ammoText.text = "";
        GameManager.instance.extractionTimerUI.text = "";

        if (GameManager.instance.currentPrimaryIcon != null)
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

        // Code for starting the transition
        if (!GameManager.instance.firstLoad)
        {
            transitionAnimator.SetTrigger("Start");
        }
        else
        {
            GameManager.instance.firstLoad = false;
        }
        currentTimer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        // This chunk of code is run when the player starts the game
        if (startGame)
        {
            if (currentTimer == 0)
            {
                transitionAnimator.SetTrigger("End");
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

    /// <summary>
    /// Starts the game, initiating the transition animation and going into the first level.
    /// </summary>
    public void StartGame()
    {
        startGame = true;
    }

    /// <summary>
    /// Changes the music volume.
    /// </summary>
    /// <param name="volume">The new volume level.</param>
    public void ChangeMusicVolume(float volume)
    {
        volumeMixer.SetFloat("musicVol", volume);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
