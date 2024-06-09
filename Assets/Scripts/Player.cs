using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Interactable
{

    Interactable currentInteractable;

    Collectible currentCollectible;

    GiftBox currentGiftBox;

    [SerializeField]
    TextMeshProUGUI interactionText;

    [SerializeField]
    Transform playerCamera;

    [SerializeField]
    float interactionDistance;

    private void Update()
    {
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, interactionDistance))
        {
            Debug.Log(hitInfo.transform.name);

            hitInfo.transform.TryGetComponent<Collectible>(out currentCollectible);
            hitInfo.transform.TryGetComponent<GiftBox>(out currentGiftBox);

            if (hitInfo.transform.TryGetComponent<Interactable>(out currentInteractable))
            {
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                currentInteractable = null;
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            currentInteractable = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    void OnInteract()
    {
        Debug.Log(currentInteractable);
        if(currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }
        if (currentCollectible != null)
        {
            currentCollectible.Interact(this);
        }
        if (currentGiftBox != null)
        {
            currentGiftBox.Interact(this);
        }
    }

}
