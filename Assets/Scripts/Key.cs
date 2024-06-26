using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    public string interactText;

    public GameObject linkedDoor;

    // Start is called before the first frame update
    void Start()
    {
        linkedDoor = GameObject.Find("Door");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        linkedDoor.GetComponent<Door>().locked = false;
        Destroy(gameObject);
    }
}
