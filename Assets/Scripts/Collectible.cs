using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Interactable
{
    [SerializeField]
    private AudioClip collectAudio;

    public int myScore = 5;

    public void Collected()
    {
        Destroy(gameObject);
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        AudioSource.PlayClipAtPoint(collectAudio, transform.position, 1f);
        GameManager.instance.IncreaseScore(myScore);
        Destroy(gameObject);
    }
}
