using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public int sceneIndex;

    public Transform player;

    public Transform targetObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //ChangeLocation2(player, targetObject); //, targetObject);
        PersistItems();
        SceneManager.LoadScene(sceneIndex);
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
