using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interface;

public class NewBehaviourScript : MonoBehaviour, Interactable
{
    public void InteractOwn()
    {
        Destroy(gameObject);
    }
}
