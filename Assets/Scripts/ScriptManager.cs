/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Script manager base class for non-interactable objects
 */

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptManager : MonoBehaviour
{
    /// <summary>
    /// Changes the background music.
    /// </summary>
    /// <param name="musicIndex">The index of the music track to play.</param>
    protected void ChangeMusic(int musicIndex)
    {
        GameManager.instance.currentBGM.Stop();
        GameManager.instance.currentBGM = GameManager.instance.BGM[musicIndex];
        GameManager.instance.currentBGM.Play();
    }

    /// <summary>
    /// Locks or unlocks the cursor.
    /// </summary>
    /// <param name="isLock">True to lock the cursor, false to unlock it.</param>
    protected void CursorLock(bool isLock)
    {
        if (isLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// Locks or unlocks player input.
    /// </summary>
    /// <param name="isLock">True to lock the player input, false to unlock it.</param>
    protected void LockInput(bool isLock)
    {
        if (isLock)
        {
            GameManager.instance.playerObject.GetComponent<FirstPersonController>().enabled = false;
            GameManager.instance.playerObject.GetComponent<PlayerInput>().enabled = false;
            GameManager.instance.playerObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GameManager.instance.playerObject.GetComponent<FirstPersonController>().enabled = true;
            GameManager.instance.playerObject.GetComponent<PlayerInput>().enabled = true;
            GameManager.instance.playerObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    /// <summary>
    /// Handles the death of an enemy.
    /// </summary>
    /// <param name="sound">The sound to play on enemy death.</param>
    /// <param name="enemy">The enemy GameObject to be destroyed.</param>
    protected virtual void KillEnemy(AudioClip sound, GameObject enemy)
    {
        AudioSource.PlayClipAtPoint(sound, enemy.transform.position);
        Destroy(enemy);
    }
}
