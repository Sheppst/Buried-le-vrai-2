using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolType : MonoBehaviour
{
    private static ToolType instance;

    [SerializeField] private Camera uicamera; 

    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("Fond").GetComponent<RectTransform>();
        tooltipText = transform.Find("Texte").GetComponent<Text>();

        ShowTooltip("Hello Worled");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uicamera, out localPoint);
        transform.localPosition = localPoint;
    }
    
    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 bacgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = bacgroundSize;
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void  ShowTooltip_Static(string tooltipString)
    {
        instance.ShowTooltip(tooltipString);
    }
    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
