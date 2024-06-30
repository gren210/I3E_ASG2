using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptManager : MonoBehaviour
{
    protected void ChangeMusic(int musicIndex)
    {
        GameManager.instance.currentBGM.Stop();
        GameManager.instance.currentBGM = GameManager.instance.BGM[musicIndex];
        GameManager.instance.currentBGM.Play();
    }

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

    protected virtual void KillEnemy(AudioClip sound, GameObject enemy)
    {
        AudioSource.PlayClipAtPoint(sound, enemy.transform.position);
        Destroy(enemy);
    }

}
