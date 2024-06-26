using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public string interactText;

    public float openDuration;

    float currentDuration;

    [SerializeField]
    Door linkedDoor;

    Vector3 startRotation;

    Vector3 targetRotation;

    bool opening;

    bool closing;

    bool opened;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    private void PullLeverDown()
    {
        if(!opening)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.z -= 45f;

            opening = true;
        }
    }

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
