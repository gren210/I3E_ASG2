/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Manages the interaction with a jump pad button
 */

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadButton : MonoBehaviour
{
    /// <summary>
    /// Called when a collider enters the trigger.
    /// </summary>
    /// <param name="other">The collider that triggered the event.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerController = other.gameObject.GetComponent<FirstPersonController>();
            var player = other.gameObject.GetComponent<Player>();

            if (playerController != null)
            {
                playerController.JumpHeight = 70f; // Increase the player's jump height.
            }

            if (player != null && player.jumpText != null)
            {
                player.jumpText.gameObject.SetActive(true); // Show jump text.
            }
        }
    }

    /// <summary>
    /// Called when acollider exits the trigger
    /// </summary>
    /// <param name="other">The collider that triggered the event.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerController = other.gameObject.GetComponent<FirstPersonController>();
            var player = other.gameObject.GetComponent<Player>();

            if (playerController != null)
            {
                playerController.JumpHeight = 1.2f; // Reset the player's jump height.
            }

            if (player != null && player.jumpText != null)
            {
                player.jumpText.gameObject.SetActive(false); // Hide jump text.
            }
        }
    }
}
