using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object : MonoBehaviour
{
    // Start is called before the first frame update

    public int targetSceneIndex = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ChangeScene();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(targetSceneIndex);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
