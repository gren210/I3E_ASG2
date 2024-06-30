/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Manages the interaction with a lever that opens and closes the linked door in level 1.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    /// <summary>
    /// Text displayed when interacting with the lever.
    /// </summary>
    public string interactText;

    /// <summary>
    /// Duration for the lever moving from start to target rotation.
    /// </summary>
    public float openDuration;

    /// <summary>
    /// Current time during the lever's animation.
    /// </summary>
    float currentDuration;

    /// <summary>
    /// The door linked to this lever.
    /// </summary>
    [SerializeField]
    Door linkedDoor;

    /// <summary>
    /// The starting rotation of the lever.
    /// </summary>
    Vector3 startRotation;

    /// <summary>
    /// The target rotation of the lever when pulled.
    /// </summary>
    Vector3 targetRotation;

    /// <summary>
    /// Checks whether the lever is currently opening.
    /// </summary>
    bool opening;

    /// <summary>
    /// Checks whether the lever is currently closing.
    /// </summary>
    bool closing;

    /// <summary>
    /// Checks whether the lever has been fully opened.
    /// </summary>
    bool opened;

  
    void Update()
    {
        if (opening)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / openDuration;
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            if (currentDuration > openDuration)
            {
                currentDuration = 0f;
                opening = false;
                opened = true;
            }
        }

        if (closing)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / openDuration;
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            if (currentDuration > openDuration)
            {
                transform.eulerAngles = targetRotation;
                currentDuration = 0f;
                closing = false;
                opened = false;
            }
        }
    }

    /// <summary>
    /// Pulls the lever down to open the linked door.
    /// </summary>
    private void PullLeverDown()
    {
        if (!opening)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.z -= 45f;

            opening = true;
        }
    }

    /// <summary>
    /// Pulls the lever up to close the linked door.
    /// </summary>
    private void PullLeverUp()
    {
        if (!closing)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.z += 45f;

            closing = true;
        }
    }

    /// <summary>
    /// Handles interaction with the lever.
    /// </summary>
    /// <param name="thePlayer">The player interacting with the lever.</param>
    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        if (!linkedDoor.opening && opened)
        {
            linkedDoor.CloseDoor();
            PullLeverUp();
        }
        else if (!linkedDoor.closing && !opened)
        {
            linkedDoor.OpenDoor();
            PullLeverDown();
        }
    }
}
