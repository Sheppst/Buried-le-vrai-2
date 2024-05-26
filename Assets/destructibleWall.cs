using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DestructibleWall : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI optionYes;
    public TextMeshProUGUI optionNo;
    private bool isPlayerNearby = false;
    private bool showMessage = false;
    private int selectedOption = 0;
    private Color baseColor = new Color(0xAD / 255f, 0x47 / 255f, 0x47 / 255f); // Couleur de base AD4747
    private Color selectedColor = new Color(1f, 0x66 / 255f, 0x66 / 255f); // Couleur de sélection FF6666

    private CharacterController2D playerController; // Référence au script CharacterController2D
    private Gun gun; // Référence au script Gun

    private bool isShowingFirstMessage = false; // Pour suivre si le premier message est affiché

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerController = FindObjectOfType<CharacterController2D>();
        gun = FindObjectOfType<Gun>();

        messagePanel.SetActive(false);
        ConfigureTextMeshPro();

        if (playerController == null)
        {
            Debug.LogError("CharacterController2D script not found in the scene.");
        }

        if (gun == null)
        {
            Debug.LogError("Gun script not found in the scene.");
        }
    }

    void ConfigureTextMeshPro()
    {
        // Configuration initiale des textes TextMeshPro
        messageText.fontSize = 24;
        optionYes.fontSize = 30;
        optionNo.fontSize = 30;
        optionYes.color = baseColor;
        optionNo.color = baseColor;
    }

    void Update()
    {
        if (isPlayerNearby && playerMovement != null && !showMessage)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerMovement.UseBomb)
                {
                    ShowMessage("Souhaitez-vous détruire ce mur ?", true);
                }
                else
                {
                    ShowMessage("Il semblerait que ce mur soit fragile..", false);
                }
            }
        }

        if (showMessage)
        {
            if (isShowingFirstMessage && Input.GetMouseButtonDown(0)) // Si le joueur clique avec la souris pour passer le premier message
            {
                HideFirstMessage();
            }
            else if (!isShowingFirstMessage)
            {
                NavigateOptions();
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    HandleOptionSelection();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            messagePanel.SetActive(false);
            showMessage = false;
            EnablePlayerControls(); // Réactiver les contrôles du joueur lorsque le joueur s'éloigne
        }
    }

    void ShowMessage(string message, bool showOptions)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
        optionYes.gameObject.SetActive(showOptions);
        optionNo.gameObject.SetActive(showOptions);
        showMessage = true;
        isShowingFirstMessage = !showOptions; // Si ce n'est pas une option, c'est le premier message
        UpdateOptionColors();
        DisablePlayerControls(); // Désactiver les contrôles du joueur lorsque le message est affiché
    }

    void HideFirstMessage()
    {
        messagePanel.SetActive(false);
        showMessage = false;
        isShowingFirstMessage = false;
        EnablePlayerControls(); // Réactiver les contrôles du joueur lorsque le premier message est passé
    }

    void NavigateOptions()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedOption = 1 - selectedOption; // Toggle between 0 and 1
            UpdateOptionColors();
        }
    }

    void UpdateOptionColors()
    {
        optionYes.color = selectedOption == 0 ? selectedColor : baseColor;
        optionNo.color = selectedOption == 1 ? selectedColor : baseColor;

        optionYes.fontSize = selectedOption == 0 ? 36 : 30;
        optionNo.fontSize = selectedOption == 1 ? 36 : 30;
    }

    void HandleOptionSelection()
    {
        if (selectedOption == 0)
        {
            Destroy(gameObject);
        }
        messagePanel.SetActive(false);
        showMessage = false;
        EnablePlayerControls(); // Réactiver les contrôles du joueur lorsque le message disparaît
    }

    void DisablePlayerControls()
    {
        if (playerController != null)
        {
            playerController.SetCanMove(false); // Désactiver le mouvement du joueur
        }

        if (gun != null)
        {
            gun.SetCanShoot(false); // Désactiver le tir
        }
    }

    void EnablePlayerControls()
    {
        if (playerController != null)
        {
            playerController.SetCanMove(true); // Réactiver le mouvement du joueur
        }

        if (gun != null)
        {
            Invoke(nameof(EnableShooting), 0.2f); // Réactiver le tir avec un léger délai
        }
    }

    void EnableShooting()
    {
        if (gun != null)
        {
            gun.SetCanShoot(true);
        }
    }
}
