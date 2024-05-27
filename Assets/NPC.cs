using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    public Dialogue[] dialogues;
    public bool autoAggro = false; // Nouveau bool�en pour activer/d�sactiver l'aggro automatique
    public bool isBoss;
    private bool isPlayerNearby = false;
    private DialogueManager dialogueManager; // R�f�rence au DialogueManager

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager script not found in the scene.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log($"{npcName} detected player nearby.");

            if (autoAggro && dialogueManager != null)
            {
                dialogueManager.StartDialogue(this);
                autoAggro = false; // D�sactiver l'auto-aggro apr�s l'aggro initial
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log($"{npcName} player left.");
        }
    }

    public bool IsPlayerNearby()
    {
        return isPlayerNearby;
    }
}