using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Interactable
{
    public string interactText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        GameManager.instance.healCount += 1;
        GameManager.instance.swapItemSound.Play();
        Destroy(gameObject);
    }
}
