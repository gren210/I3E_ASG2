/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Manages the interaction with a key that unlocks a linked door in level 2.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    /// <summary>
    /// Text displayed when interacting with the key.
    /// </summary>
    public string interactText;

    /// <summary>
    /// The door linked to this key.
    /// </summary>
    public GameObject linkedDoor;

    /// <summary>
    /// Sound played when the key is picked up.
    /// </summary>
    [SerializeField]
    AudioClip pickupSound;

    void Start()
    {
        linkedDoor = GameObject.Find("Door");
    }

    /// <summary>
    /// Handles interaction with the key.
    /// </summary>
    /// <param name="thePlayer">The player interacting with the key.</param>
    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        linkedDoor.GetComponent<Door>().locked = false;
        Destroy(gameObject);
    }
}

