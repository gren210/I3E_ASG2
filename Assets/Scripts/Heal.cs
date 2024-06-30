/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Script for the medkit gameObject.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Interactable
{
    /// <summary>
    /// Runs when the player interacts with the medkit.
    /// </summary>
    /// <param name="thePlayer">The player interacting with the heal item.</param>
    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        GameManager.instance.healCount += 1; // Increase the heal count.
        GameManager.instance.swapItemSound.Play(); // Play the item swap sound.
        Destroy(gameObject); // Destroy the heal item after interaction.
    }
}
