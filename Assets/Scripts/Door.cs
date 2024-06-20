using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    bool opened;

    public float openDuration;

    float currentDuration;

    bool opening = false;

    bool closing = false;

    public float rotationX;

    public float rotationY;

    public float rotationZ;

    Vector3 startRotation;

    Vector3 targetRotation;

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

            if(currentDuration >= openDuration)
            {
                currentDuration = 0f;
                transform.eulerAngles = targetRotation;
                opening = false;
                opened = true;
            }

        }
        if (closing)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / openDuration;
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);

            if (currentDuration >= openDuration)
            {
                currentDuration = 0f;
                transform.eulerAngles = targetRotation;
                closing = false;
                opened = false;
            }

        }

    }

    public void OpenDoor()
    {
        if (!opening)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.x += rotationX;
            targetRotation.y += rotationY;
            targetRotation.z += rotationZ;

            opening = true;
        }
    }

    public void CloseDoor()
    {
        if (!closing)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.x -= rotationX;
            targetRotation.y -= rotationY;
            targetRotation.z -= rotationZ;

            closing = true;
        }
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer); 
        if (opened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }

    }

}
