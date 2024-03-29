using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyInterface;

public class PNJDialogue : MonoBehaviour, Interactable
{
    public void Interact()
    {
        print("Bonjour aventurier, allez sauver ma fille svp");
    }
}
