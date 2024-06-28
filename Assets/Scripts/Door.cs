using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Door : Interactable
{
    public string interactText;

    [HideInInspector]
    public bool opened;

    public bool locked;

    public bool isInteractable;

    public float openDuration;

    float currentDuration;

    [SerializeField]
    AudioSource openingSound;

    [HideInInspector]
    public bool opening = false;

    [HideInInspector]
    public bool closing = false;

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

    [SerializeField]
    bool extraction;

    [SerializeField]
    float extractionTime;

    [HideInInspector]
    public float currentExtractionTimer;

    bool extracted = false;

    // Start is called before the first frame update
    void Start()
    {
        currentExtractionTimer = extractionTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            if (GameManager.instance.currentPrimary != null && GameManager.instance.currentGrenade != null && GameManager.instance.healCount > 0)
            {
                Debug.Log(GameManager.instance.currentPrimary);
                locked = false;
            }
        }

        if (extraction && !extracted)
        {
            currentExtractionTimer -= Time.deltaTime;
            GameManager.instance.extractionTimerUI.text = "" + (int) currentExtractionTimer;
            if (currentExtractionTimer <= 0)
            {
                locked = false;
                OpenDoor();
                extracted = true;
            }
                
        }

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
                openingSound.Stop();
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
                openingSound.Stop();
            }

        }
    }

    public void OpenDoor()
    {
        if (!opening)
        {
            openingSound.Play();
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
            openingSound.Play();
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
        if (isInteractable && !locked)
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

    

}
