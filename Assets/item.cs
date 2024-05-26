using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemDescription;
    public bool autoAggro = false;
    public bool isBomb = false; // Ajouter ce drapeau pour diff�rencier les �l�ments de la bombe

    private bool isPlayerNearby = false;
    private ItemBoxManager itemBoxManager;
    private PlayerMovement playerMovement; // R�f�rence � PlayerMovement

    void Start()
    {
        itemBoxManager = FindObjectOfType<ItemBoxManager>();
        playerMovement = FindObjectOfType<PlayerMovement>(); // Trouver PlayerMovement
        if (itemBoxManager == null)
        {
            Debug.LogError("Script ItemBoxManager non trouv� dans la sc�ne.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Joueur d�tect� � proximit�.");

            if (autoAggro && itemBoxManager != null)
            {
                itemBoxManager.ShowItemBox(itemDescription);
                autoAggro = false;
                HandleItemPickup(); // Appeler la nouvelle m�thode pour g�rer la collecte de l'�l�ment
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Le joueur est parti.");
        }
    }

    public bool IsPlayerNearby()
    {
        return isPlayerNearby;
    }

    private void HandleItemPickup()
    {
        if (isBomb && playerMovement != null)
        {
            playerMovement.EnableBombUsage(); // Activer l'utilisation de la bombe
        }
        Destroy(gameObject); // D�truire l'�l�ment apr�s ramassage
    }
}
