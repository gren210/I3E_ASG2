using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{

    public virtual void Interact(Player thePlayer)
    {
        
    }

    protected virtual void PickupCollectible(GameObject collectible, int iconIndex, GameObject currentCollectible)
    {
        if (GameManager.instance.currentEquippable != null && GameManager.instance.currentEquippable != currentCollectible)
        {
            GameManager.instance.currentEquippable.SetActive(false);
        }

        collectible.transform.SetParent(GameManager.instance.playerCamera.transform, false);
        collectible.GetComponent<Rigidbody>().isKinematic = true;

        GameManager.instance.currentEquippable = collectible;
        GameManager.instance.currentEquippable.SetActive(true);

        collectible.transform.position = GameManager.instance.equipPosition.transform.position;
        collectible.transform.eulerAngles = GameManager.instance.equipPosition.transform.eulerAngles;

        GameManager.instance.swapItemSound.Play();
    }

    protected void ShakeCamera(float shakeIntensity, float shakeFrequency)
    {
        CinemachineBasicMultiChannelPerlin cinemachineComponent = GameManager.instance.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineComponent.m_AmplitudeGain = shakeIntensity;
        cinemachineComponent.m_FrequencyGain = shakeFrequency;
    }

    protected void UpdateEnemyHealth(GameObject enemy, float damage)
    {
        if (enemy.TryGetComponent<Boss>(out Boss currentBoss))
        {
            currentBoss.currentEnemyHealth -= damage;
            currentBoss.healthBarGreen.fillAmount = currentBoss.currentEnemyHealth / currentBoss.enemyHealth;
        }
        else if(enemy.TryGetComponent<Enemy>(out Enemy currentEnemy))
        {
            currentEnemy.currentEnemyHealth -= damage;
            currentEnemy.healthBarGreen.fillAmount = currentEnemy.currentEnemyHealth / currentEnemy.enemyHealth;
        }
    }

    protected void ChangeMusic(int musicIndex)
    {
        GameManager.instance.currentBGM.Stop();
        GameManager.instance.currentBGM = GameManager.instance.BGM[musicIndex];
        GameManager.instance.currentBGM.Play();
    }




}
