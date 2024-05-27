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
    private CharacterController2D playerController;
    private bool isDialogueActive = false;
    private bool isBossDialogue = false; // Ajoutez cette ligne
    private Boss boss;
    private Gun gun;

    public bool IsDialogueActive => isDialogueActive;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueUI.SetActive(false);

        playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract == null)
        {
            Debug.LogError("PlayerInteract script not found in the scene.");
        }

        playerController = FindObjectOfType<CharacterController2D>();
        if (playerController == null)
        {
            Debug.LogError("CharacterController2D script not found in the scene.");
        }

        boss = FindObjectOfType<Boss>();
        if (boss == null)
        {
            Debug.LogError("Boss script not found in the scene.");
        }

        gun = FindObjectOfType<Gun>();
        if (gun == null)
        {
            Debug.LogError("Gun script not found in the scene.");
        }
    }

    public void StartDialogue(NPC npc) // Modifiez cette ligne
    {
        if (isDialogueActive) return;

        isDialogueActive = true;
        isBossDialogue = npc.isBoss; // Ajoutez cette ligne

        if (playerInteract != null)
        {
            playerInteract.SetInteractionMessageActive(false);
        }

        if (playerController != null)
        {
            playerController.SetCanMove(false);
        }

        dialogueUI.SetActive(true);
        nameText.text = npc.npcName;
        sentences.Clear();

        lines = npc.dialogues[0].sentences;
        index = 0;

        foreach (string sentence in lines)
        {
            sentences.Enqueue(sentence);
        }

        if (gun != null)
        {
            gun.SetCanShoot(false);
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
        isDialogueActive = false;

        if (playerInteract != null)
        {
            playerInteract.SetInteractionMessageActive(true);
        }

        if (playerController != null)
        {
            playerController.SetCanMove(true);
        }

        if (gun != null)
        {
            Invoke(nameof(EnableShooting), 0.2f);
        }

        if (isBossDialogue && boss != null) // Modifiez cette ligne
        {
            boss.GoToIdleState();
        }

        Debug.Log("Dialogue ended.");
    }

    void EnableShooting()
    {
        gun.SetCanShoot(true);
    }
}
