using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask npcLayer;
    public TextMeshProUGUI interactionMessage; // Ensure this is assigned in the Inspector
    public Vector3 offset = new Vector3(0, 50, 0); // Offset for positioning the message
    private DialogueManager dialogueManager;
    private bool isPlayerNearby = false;
    private Transform currentNPC; // Reference to the current NPC's transform

    void Start()
    {
        Debug.Log("PlayerInteraction Start() called.");

        GameObject dialogueManagerObject = GameObject.FindGameObjectWithTag("DialogueManager");
        if (dialogueManagerObject != null)
        {
            dialogueManager = dialogueManagerObject.GetComponent<DialogueManager>();
            Debug.Log("DialogueManager found and assigned.");
        }
        else
        {
            Debug.LogError("DialogueManager not found. Make sure there is a DialogueManager in the scene with the correct tag.");
        }

        if (interactionMessage == null)
        {
            Debug.LogError("interactionMessage is not assigned. Please assign it in the Inspector.");
        }

        interactionMessage.gameObject.SetActive(false); // Ensure the message is initially hidden
    }

    void Update()
    {
        CheckForNPCProximity();

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithNPC();
        }

        if (interactionMessage.gameObject.activeSelf && currentNPC != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(currentNPC.position);
            interactionMessage.transform.position = screenPosition + offset; // Apply the offset
        }
    }

    void CheckForNPCProximity()
    {
        Collider2D[] npcs = Physics2D.OverlapCircleAll(transform.position, interactionRange, npcLayer);
        if (npcs.Length > 0)
        {
            NPC npc = npcs[0].GetComponent<NPC>();
            if (npc != null && npc.IsPlayerNearby())
            {
                interactionMessage.gameObject.SetActive(true); // Show the message
                isPlayerNearby = true;
                currentNPC = npc.transform; // Keep reference to the current NPC
                return;
            }
        }

        interactionMessage.gameObject.SetActive(false); // Hide the message
        isPlayerNearby = false;
        currentNPC = null; // Clear the reference
    }

    void InteractWithNPC()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager is not assigned.");
            return;
        }

        Collider2D[] npcs = Physics2D.OverlapCircleAll(transform.position, interactionRange, npcLayer);
        foreach (Collider2D npcCollider in npcs)
        {
            NPC npc = npcCollider.GetComponent<NPC>();
            if (npc != null)
            {
                Debug.Log($"NPC found: {npc.npcName}, IsPlayerNearby: {npc.IsPlayerNearby()}");
                if (npc.IsPlayerNearby())
                {
                    dialogueManager.StartDialogue(npc);
                    Debug.Log("Dialogue started with NPC.");
                    break;
                }
            }
        }
        Debug.Log("Interaction attempted.");
    }

    public void SetInteractionMessageActive(bool isActive)
    {
        if (interactionMessage != null)
        {
            interactionMessage.gameObject.SetActive(isActive);
        }
        else
        {
            Debug.LogError("interactionMessage is not assigned.");
        }
    }
}