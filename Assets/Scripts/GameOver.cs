using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
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
        transition = GameManager.instance.transition;
        transitionAnimator = GameManager.instance.transitionAnimator;
        currentTimer = 0;
    }

    // Update is called once per frame
    void Update()
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
                GameManager.instance.gameOverUI.SetActive(false);
                goBack = false;
                currentTimer = 0;
                transitionPlayed = false;
                GameManager.instance.UI.SetActive(false);
                GameManager.instance.currentBGM.Stop();
                SceneManager.LoadScene(0);

            }
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        goBack = true;
    }


}
