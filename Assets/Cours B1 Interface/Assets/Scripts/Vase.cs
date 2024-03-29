using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyInterface;

public class Vase : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Destroy(gameObject);
    }
}
