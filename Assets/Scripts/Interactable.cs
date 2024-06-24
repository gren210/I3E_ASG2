using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public virtual void Interact(Player thePlayer)
    {
        Debug.Log(gameObject.name + " was interacted with.");
    }

    public virtual void ShakeCamera(float shakeIntensity, float shakeFrequency)
    {
        CinemachineBasicMultiChannelPerlin cinemachineComponent = GameManager.instance.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineComponent.m_AmplitudeGain = shakeIntensity;
        cinemachineComponent.m_FrequencyGain = shakeFrequency;
    }

    public virtual void PositiveLerp(float posX, float posY, float posZ, float rotX, float rotY, float rotZ)
    {

    }

}
