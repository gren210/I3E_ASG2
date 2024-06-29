using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadButton : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FirstPersonController>().JumpHeight = 70f;
            other.gameObject.GetComponent<Player>().jumpText.gameObject.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FirstPersonController>().JumpHeight = 1.2f;
            other.gameObject.GetComponent<Player>().jumpText.gameObject.SetActive(false);
        }

    }
}
