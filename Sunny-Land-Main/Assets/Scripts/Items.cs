using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] // set a Collider2D for the Object

public class Items : MonoBehaviour
{
    public enum InteractionType {NONE, PickUp, Examine}
    public InteractionType type;

    private void Reset() 
    {
        GetComponent<Collider2D>().isTrigger = true; // set the Collider2D to be a trigger
        gameObject.layer = 9; // set the Object to the "Item" layer
    }

    public void Interact()
    {
        switch(type)
        {
            case InteractionType.PickUp:
                GameObject item = gameObject;
                FindObjectOfType<InteractionSystem>().PickUpItem(gameObject);
                gameObject.SetActive(false);
                Debug.Log("pickup");
                break;
            case InteractionType.Examine:
                Debug.Log("examine");
                break;
            default:
                Debug.Log("null");
                break;
        }
    }
}
