/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Script for the base class for interactable objects in the game.
 */

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// Virtual function for interacting with the player.
    /// </summary>
    /// <param name="thePlayer">The player interacting with the object.</param>
    public virtual void Interact(Player thePlayer)
    {
        
    }

    /// <summary>
    /// Handles picking up a collectible item and equipping it to the player. Meant for the primary and grenade.
    /// </summary>
    /// <param name="collectible">The collectible item to be picked up.</param>
    /// <param name="iconIndex">The index of the icon representing the collectible.</param>
    /// <param name="currentCollectible">The currently equipped collectible.</param>
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

    /// <summary>
    /// Shakes the camera.
    /// </summary>
    /// <param name="shakeIntensity">The intensity of the shake.</param>
    /// <param name="shakeFrequency">The frequency of the shake.</param>
    protected void ShakeCamera(float shakeIntensity, float shakeFrequency)
    {
        CinemachineBasicMultiChannelPerlin cinemachineComponent = GameManager.instance.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineComponent.m_AmplitudeGain = shakeIntensity;
        cinemachineComponent.m_FrequencyGain = shakeFrequency;
    }

    /// <summary>
    /// Updates the health of an enemy after taking damage.
    /// </summary>
    /// <param name="enemy">The enemy GameObject.</param>
    /// <param name="damage">The amount of damage dealt.</param>
    protected void UpdateEnemyHealth(GameObject enemy, float damage)
    {
        if (enemy.TryGetComponent<Boss>(out Boss currentBoss))
        {
            currentBoss.currentEnemyHealth -= damage;
            currentBoss.healthBarGreen.fillAmount = currentBoss.currentEnemyHealth / currentBoss.enemyHealth;
        }
        else if (enemy.TryGetComponent<Enemy>(out Enemy currentEnemy))
        {
            currentEnemy.currentEnemyHealth -= damage;
            currentEnemy.healthBarGreen.fillAmount = currentEnemy.currentEnemyHealth / currentEnemy.enemyHealth;
        }
    }

    /// <summary>
    /// Changes the background music to a specified track.
    /// </summary>
    /// <param name="musicIndex">The index of the music track to play.</param>
    protected void ChangeMusic(int musicIndex)
    {
        GameManager.instance.currentBGM.Stop();
        GameManager.instance.currentBGM = GameManager.instance.BGM[musicIndex];
        GameManager.instance.currentBGM.Play();
    }
}

