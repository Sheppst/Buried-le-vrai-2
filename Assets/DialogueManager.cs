using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueUI;
    public float textSpeed = 0.05f;

    private Queue<string> sentences;
    private int index;
    private string[] lines;
    private PlayerInteract playerInteract;
    private bool isDialogueActive = false; // Track whether a dialogue is active
    private Boss boss;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueUI.SetActive(false); // Ensure the dialogue UI is initially inactive

        // Find the PlayerInteract script in the scene
        playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract == null)
        {
            Debug.LogError("PlayerInteract script not found in the scene.");
        }

        // Find the Boss script in the scene
        boss = FindObjectOfType<Boss>();
        if (boss == null)
        {
            Debug.LogError("Boss script not found in the scene.");
        }
    }

    public void StartDialogue(NPC npc)
    {
        if (isDialogueActive) return; // Prevent starting a new dialogue if one is active

        isDialogueActive = true;

        if (playerInteract != null)
        {
            playerInteract.SetInteractionMessageActive(false); // Disable interaction message
        }

        dialogueUI.SetActive(true);
        nameText.text = npc.npcName;
        sentences.Clear();

        lines = npc.dialogues[0].sentences; // Assuming one dialogue for simplicity
        index = 0;

        foreach (string sentence in lines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        Debug.Log($"Started dialogue with {npc.npcName}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialogueUI.activeSelf)
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }
        }
    }

    void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeSentence(lines[index]));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = string.Empty;
        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isDialogueActive = false; // Reset dialogue active state

        if (playerInteract != null)
        {
            playerInteract.SetInteractionMessageActive(true); // Re-enable interaction message
        }

        // Notifier le boss que le dialogue est terminé
        if (boss != null)
        {
            boss.GoToIdleState();
        }

        Debug.Log("Dialogue ended.");
    }
}