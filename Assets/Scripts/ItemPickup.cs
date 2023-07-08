using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable 
{
    public override void Interact() 
    { 
        base.Interact();

        PickUp();
    }

    public void PickUp()
    {
        Debug.Log("Picking Up Item");
        Destroy(gameObject);
    }
}
