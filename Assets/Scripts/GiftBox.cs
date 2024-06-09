using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : Interactable
{
    [SerializeField]
    private GameObject collectibleToSpawn;

    [SerializeField]
    private AudioClip collectAudio;

    public override void Interact(Player thePlayer)
    {
        AudioSource.PlayClipAtPoint(collectAudio, transform.position, 1f);
        base.Interact(thePlayer);
        SpawnCollectible();
        Destroy(gameObject);
    }

    void SpawnCollectible()
    {
        Instantiate(collectibleToSpawn, transform.position, collectibleToSpawn.transform.rotation);
    }

}
