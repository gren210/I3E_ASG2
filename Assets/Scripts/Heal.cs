using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Interactable
{
    public string interactText;

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        GameManager.instance.healCount += 1;
        GameManager.instance.swapItemSound.Play();
        Destroy(gameObject);
    }
}
