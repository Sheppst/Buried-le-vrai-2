using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemDescription;
    public bool autoAggro = false;
    public bool isBomb = false; // Ajouter ce drapeau pour diff�rencier les �l�ments de la bombe
    public bool isLifePotion = false; // Ajouter ce drapeau pour diff�rencier les potions de vie
    public bool isManaPotion = false; // Ajouter ce drapeau pour diff�rencier les potions de mana

    private bool isPlayerNearby = false;
    private ItemBoxManager itemBoxManager;
    private PlayerMovement playerMovement; // R�f�rence � PlayerMovement
    private Inventory playerInventory; // R�f�rence � l'inventaire du joueur

    void Start()
    {
        itemBoxManager = FindObjectOfType<ItemBoxManager>();
        playerMovement = FindObjectOfType<PlayerMovement>(); // Trouver PlayerMovement
        playerInventory = FindObjectOfType<Inventory>(); // Trouver l'inventaire du joueur
        if (itemBoxManager == null)
        {
            Debug.LogError("Script ItemBoxManager non trouv� dans la sc�ne.");
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (itemBoxManager != null)
            {
                itemBoxManager.ShowItemBox(itemDescription);
                HandleItemPickup(); // Appeler la m�thode pour g�rer la collecte de l'�l�ment
            }
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
                HandleItemPickup(); // Appeler la m�thode pour g�rer la collecte de l'�l�ment
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
        if (isLifePotion && playerInventory != null)
        {
            playerInventory.AddLifePotion(); // Ajouter une potion de vie
        }
        if (isManaPotion && playerInventory != null)
        {
            playerInventory.AddManaPotion(); // Ajouter une potion de mana
        }
        Destroy(gameObject); // D�truire l'�l�ment apr�s ramassage
    }
}