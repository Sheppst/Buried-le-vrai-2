using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBoxManager : MonoBehaviour
{
    public GameObject itemBoxUI;
    public TextMeshProUGUI itemDescriptionText;
    private bool isItemBoxActive = false;

    void Start()
    {
        if (itemBoxUI == null)
        {
            Debug.LogError("UI de la boîte d'objets non assignée.");
        }
        else
        {
            itemBoxUI.SetActive(false); // Assurez-vous que l'UI de la boîte d'objets est initialement inactive
        }
    }

    public void ShowItemBox(string itemDescription)
    {
        if (isItemBoxActive) return;

        isItemBoxActive = true;
        itemDescriptionText.text = itemDescription;
        itemBoxUI.SetActive(true);
    }

    public void HideItemBox()
    {
        if (!isItemBoxActive) return;

        isItemBoxActive = false;
        itemBoxUI.SetActive(false);
    }

    void Update()
    {
        if (isItemBoxActive && Input.GetMouseButtonDown(0))
        {
            HideItemBox();
        }
    }
}
