using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Door : Interactable
{
    bool opened;

    public bool locked;

    public float openDuration;

    float currentDuration;

    bool opening = false;

    bool closing = false;

    public float rotationX;

    public float rotationY;

    public float rotationZ;

    public float positionX;

    public float positionY;

    public float positionZ;

    Vector3 startRotation;

    Vector3 targetRotation;

    Vector3 startPosition;

    Vector3 targetPosition;

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
            if (rotationX + rotationY + rotationZ != 0f)
            {
                transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            }
            if (positionX + positionY + positionZ != 0f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }

            if (currentDuration >= openDuration)
            {
                currentDuration = 0f;
                transform.eulerAngles = targetRotation;
                transform.position = targetPosition;
                opening = false;
                opened = true;
            }

        }
        if (closing)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / openDuration;
            if (rotationX + rotationY + rotationZ != 0f )
            {
                transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            }
            if (positionX + positionY + positionZ != 0f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }

            if (currentDuration >= openDuration)
            {
                currentDuration = 0f;
                transform.eulerAngles = targetRotation;
                transform.position = targetPosition;
                closing = false;
                opened = false;
            }

        }

        if (gameObject.name == "DoorHinge")
        {
            locked = false;
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

            startPosition = transform.position;
            targetPosition = startPosition;
            targetPosition.x += positionX;
            targetPosition.y += positionY;
            targetPosition.z += positionZ;

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

            startPosition = transform.position;
            targetPosition = startPosition;
            targetPosition.x -= positionX;
            targetPosition.y -= positionY;
            targetPosition.z -= positionZ;

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
