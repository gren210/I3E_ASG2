using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
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
        GameManager.instance.UpdateHealth();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.instance.BGM[sceneIndex - 1].Play();
        GameManager.instance.currentBGM = GameManager.instance.BGM[sceneIndex - 1];
        currentTimer = 0;
        changeScene = false;
        transition = GameManager.instance.transition;
        transitionAnimator = GameManager.instance.transitionAnimator;
        if(!firstScene )
        {
            transitionAnimator.SetTrigger("Start");
        }
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            GameManager.instance.UI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (changeScene)
        {
            if (currentTimer == 0)
            {
                transitionAnimator.SetTrigger("End");
                GameManager.instance.playerObject.GetComponent<FirstPersonController>().enabled = false;
                GameManager.instance.playerObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            currentTimer += Time.deltaTime;
            if (currentTimer >= transitionTime)
            {
                GameManager.instance.BGM[sceneIndex - 1].Stop();
                if(enemies != null)
                {
                    Destroy(enemies);
                }
                SceneManager.LoadScene(sceneIndex);
                PersistItems();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        changeScene = true;
    }

    private void PersistItems()
    {
        if (GameManager.instance.currentPrimary != null)
        {
            GameManager.instance.currentPrimary.transform.SetParent(GameManager.instance.gameObject.transform, false);
        }
        if (GameManager.instance.currentGrenade != null)
        {
            GameManager.instance.currentGrenade.transform.SetParent(GameManager.instance.gameObject.transform, false);
        }
    }
}
