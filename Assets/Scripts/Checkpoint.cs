using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    Door linkedDoor;

    bool hasClosedDoor = false;

    bool checkpointDone = false;

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
        if(other.transform.tag == "Player" && !checkpointDone)
        {
            GameManager.instance.currentCheckpoint = gameObject;
            checkpointDone = true;
            if (!hasClosedDoor)
            {
                linkedDoor.CloseDoor();
                hasClosedDoor = true;
            }
        }
    }


}
