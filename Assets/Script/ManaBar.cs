using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManaBar : MonoBehaviour
{
    private Image barImage;
    private Mana mana;
    void Awake()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        mana = player.GetComponent<Mana>();

    }

    private void Update()
    {
        mana.Update();
        barImage.fillAmount = mana.ManaNormalized();
        
    }
      
}

