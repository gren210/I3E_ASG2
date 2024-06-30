/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Handles the interaction of placing a crystal in the spaceship.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCrystal : Interactable
{
    /// <summary>
    /// Text displayed when interacting with the place crystal object.
    /// </summary>
    public string interactText;

    /// <summary>
    /// The crystal to spawn.
    /// </summary>
    [SerializeField]
    GameObject crystal;

    /// <summary>
    /// The position where the crystal spawns.
    /// </summary>
    [SerializeField]
    Transform crystalPosition;

    /// <summary>
    /// The door that unlocks when the crystal spawns.
    /// </summary>
    [SerializeField]
    Door linkedDoor;

    /// <summary>
    /// The audio source used to play the crystal spawn sound.
    /// </summary>
    [SerializeField]
    AudioSource placeCrystalSound;

    /// <summary>
    /// Handles interaction with the place crystal object (reactor)
    /// </summary>
    /// <param name="thePlayer">The player interacting with the place crystal object.</param>
    public override void Interact(Player thePlayer)
    {
        GameManager.instance.objectiveText.text = GameManager.instance.objectiveStrings[10];
        base.Interact(thePlayer);
        placeCrystalSound.Play();
        Instantiate(crystal, crystalPosition.position, crystalPosition.rotation);
        linkedDoor.locked = false;
    }
}
