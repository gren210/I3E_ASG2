using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    Vector3 jumpDirection;

    [SerializeField]
    float jumpForce;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log(jumpForce);
            Vector3 jumpDirection = other.transform.up;
            jumpDirection *= 1000f;
            other.gameObject.GetComponent<Rigidbody>().AddForce(jumpDirection, ForceMode.Impulse);
        }
    }

}
