using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool isDoorOpen = false;
    public bool openHorizontally = false; // Détermine si la porte s'ouvre horizontalement
    public bool openVertically = false;   // Détermine si la porte s'ouvre verticalement
    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    [SerializeField] float doorSpeed;
    [SerializeField] float doorHigh;
    [SerializeField] float doorWidth; // Ajouté pour l'ouverture horizontale

    void Awake()
    {
        doorClosedPos = transform.position;
        if (openHorizontally)
        {
            doorOpenPos = new Vector3(transform.position.x + doorWidth, transform.position.y, transform.position.z);
        }
        else if (openVertically)
        {
            doorOpenPos = new Vector3(transform.position.x, transform.position.y + doorHigh, transform.position.z);
        }
    }

    void Update()
    {
        if (isDoorOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (transform.position != doorOpenPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);
        }
    }

    void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
        }
    }

    void OnValidate()
    {
        // Assurez-vous que seulement un des deux booléens est activé
        if (openHorizontally && openVertically)
        {
            Debug.LogWarning("La porte ne peut pas s'ouvrir horizontalement et verticalement en même temps. Veuillez choisir une seule direction.");
            openVertically = false; // Désactiver l'ouverture verticale par défaut si les deux sont activés
        }
    }
}

