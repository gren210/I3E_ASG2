using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public float openDuration;

    float currentDuration;

    [SerializeField]
    Door linkedDoor;

    Vector3 startRotation;

    Vector3 targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (linkedDoor.opening || linkedDoor.closing)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / openDuration;
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            if (currentDuration > openDuration)
            {
                currentDuration = 0f;

            }
        }

    }

    private void PullLeverDown()
    {
        if(!linkedDoor.opening)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.z -= 45f;
        }
    }

    private void PullLeverUp()
    {
        if (!linkedDoor.closing)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.z += 45f;
        }
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        if (linkedDoor.opened)
        {
            linkedDoor.CloseDoor();
            PullLeverUp();
        }
        else
        {
            linkedDoor.OpenDoor();
            PullLeverDown();
        }
    }
}
