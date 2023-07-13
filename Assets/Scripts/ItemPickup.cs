
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : Interactable 
{
    Transform player;
    
    public void click(InputAction.CallbackContext ctx)
    {
        PickUp();
    }

    public Item item;
    public override void Interact() 
    { 
        base.Interact();

        PickUp();
    }

    public void PickUp()
    {
        float distance = Vector3.Distance(player.position, interactionTransform.position);
        if(distance <= radius){
            Debug.Log("Picking Up " + item.name);
            Destroy(gameObject);
        }
    }
}
