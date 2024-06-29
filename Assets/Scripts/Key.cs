using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    public string interactText;

    public GameObject linkedDoor;

    [SerializeField]
    AudioClip pickupSound;

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
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        linkedDoor.GetComponent<Door>().locked = false;
        Destroy(gameObject);
    }
}
