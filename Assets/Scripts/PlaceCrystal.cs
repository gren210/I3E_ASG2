using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCrystal : Interactable
{
    [SerializeField]
    GameObject crystal;

    [SerializeField]
    Transform crystalPosition;

    [SerializeField]
    Door linkedDoor;

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
        Instantiate(crystal, crystalPosition.position, crystalPosition.rotation);
        linkedDoor.locked = false;
    }

}
