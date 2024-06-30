using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : ScriptManager
{
    public int sceneIndex;

    GameObject transition;

    Animator transitionAnimator;

    [SerializeField]
    float transitionTime;

    float currentTimer;

    bool changeScene;

    [SerializeField]
    bool firstScene;

    [SerializeField]
    GameObject enemies;



    // Start is called before the first frame update
    void Start()
    {
        //transitionAnimator.SetTrigger("Start");
        GameManager.instance.isImmune = false;
        AudioListener.pause = false;

        //if(GameManager.instance.currentPrimary != null)
        //{
            //GameManager.instance.savedPrimary = GameManager.instance.primaryPrefabs[GameManager.instance.currentPrimary.GetComponent<Gun>().iconIndex];
        //}

        //if (GameManager.instance.currentGrenade != null)
        //{
            //GameManager.instance.savedGrenade = GameManager.instance.grenadePrefabs[GameManager.instance.currentGrenade.GetComponent<Grenade>().iconIndex];
        //}

        GameManager.instance.currentScene = SceneManager.GetActiveScene().buildIndex;
        GameManager.instance.savedHealCount = GameManager.instance.healCount;
        GameManager.instance.objectiveText.text = GameManager.instance.objectiveStrings[GameManager.instance.currentScene];
        GameManager.instance.currentCheckpoint = null;
        GameManager.instance.currentBGM = GameManager.instance.BGM[sceneIndex - 1];
        GameManager.instance.currentBGM.Play();
        
        GameManager.instance.UpdateHealth();
        CursorLock(true);
        
        currentTimer = 0;
        changeScene = false;
        transition = GameManager.instance.transition;
        transitionAnimator = GameManager.instance.transitionAnimator;
        if(!firstScene || GameManager.instance.hasRestarted)
        {
            transitionAnimator.SetTrigger("Start");
        }
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            GameManager.instance.objectiveText.text = GameManager.instance.objectiveStrings[0];
            GameManager.instance.UI.SetActive(true);
            //GameManager.instance.currentBGM.Stop();
            //GameManager.instance.currentBGM = GameManager.instance.BGM[5];
            //GameManager.instance.currentBGM.Play();
            ChangeMusic(5);
        }
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            //GameManager.instance.currentBGM.Stop();
            //GameManager.instance.currentBGM = GameManager.instance.BGM[8];
            //GameManager.instance.currentBGM.Play();
            ChangeMusic(8);
            GameManager.instance.UI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (changeScene)
        {
            GameManager.instance.isImmune = true;
            if (currentTimer == 0)
            {
                transitionAnimator.SetTrigger("End");
                GameManager.instance.playerObject.GetComponent<FirstPersonController>().enabled = false;
                GameManager.instance.playerObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            currentTimer += Time.deltaTime;
            if (currentTimer >= transitionTime)
            {
                GameManager.instance.currentBGM.Stop();
                if(enemies != null)
                {
                    Destroy(enemies);
                }
                SceneManager.LoadScene(sceneIndex);
                GameManager.instance.PersistItems();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            changeScene = true;
        }
    }

    //private void PersistItems()
    //{
        //if (GameManager.instance.currentPrimary != null)
        //{
            //GameManager.instance.currentPrimary.transform.SetParent(GameManager.instance.gameObject.transform, false);
        //}
        //if (GameManager.instance.currentGrenade != null)
        //{
            //GameManager.instance.currentGrenade.transform.SetParent(GameManager.instance.gameObject.transform, false);
        //}
    //}
}
