using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public int sceneIndex;

    public Transform player;

    private Vector3 currentPosition;

    private Quaternion currentRotation;

    public float positionX;

    public float positionY;

    public float positionZ;

    public float rotationX;

    public float rotationY;

    public float rotationZ;

    public Transform targetObject;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(sceneIndex);
                Debug.Log("yes");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeLocation2(player, targetObject); //, targetObject);
        SceneManager.LoadScene(sceneIndex);
        timer = Time.deltaTime;
    }

    private void ChangeLocation2(Transform player, Transform target)
    {
        currentPosition = target.position;
        currentRotation = target.rotation;
        player.position = currentPosition;
        Physics.SyncTransforms();
        player.rotation = currentRotation;
    }

}
