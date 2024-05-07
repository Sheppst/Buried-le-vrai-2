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
        mana = new Mana();
        
    }

    private void Update()
    {
        mana.Update();
        barImage.fillAmount = mana.ManaNormalized();
        
    }
      
}
public class Mana : MonoBehaviour
{

    private float manaPool;
    private const float maxMana = 100f;
    private float manaRegen;

    public Mana()
    {
        manaPool = 0f;
        manaRegen = 30f;
    }

    public void Update()
    {
        manaPool += manaRegen * Time.deltaTime;
        manaPool = Mathf.Clamp(manaPool, 0f, maxMana);
    }
    public void SpendMana(int amount)
    {
        if (manaPool >= amount)
        {
            manaPool -= amount;
        }
    }

    public float ManaNormalized()
    {
        return manaPool / maxMana;
    }
}
