using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCrystal : Interactable
{
    public string interactText;

    [SerializeField]
    GameObject crystal;

    [SerializeField]
    Transform crystalPosition;

    [SerializeField]
    Door linkedDoor;

    [SerializeField]
    AudioSource placeCrystalSound;

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
        GameManager.instance.objectiveText.text = GameManager.instance.objectiveStrings[10];
        base.Interact(thePlayer);
        placeCrystalSound.Play();
        Instantiate(crystal, crystalPosition.position, crystalPosition.rotation);
        linkedDoor.locked = false;
    }

}
