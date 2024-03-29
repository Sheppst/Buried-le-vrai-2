using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyInterface;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Interactable CollisionInteract = collision.GetComponent<Interactable>();

        if(CollisionInteract != null)
        {
            CollisionInteract.Interact();
        }

    }
}
