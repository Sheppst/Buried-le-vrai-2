using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{

    public float manaPool;
    [HideInInspector] public const float maxMana = 100f;
    [SerializeField] public float manaRegen;
    public bool regenDelay = true;

    public Mana()
    {
        manaPool = 0f;
        
    }

    public void RegenerateMana()
    {
        if (regenDelay == true)
        {
            manaRegen = 24f;
            manaPool += manaRegen * Time.deltaTime;
            manaPool = Mathf.Clamp(manaPool, 0f, maxMana);
        }else
        {
            manaRegen = 0;
           
        }

            
        }
    IEnumerator Delay()
    {
        regenDelay = false;
        yield return new WaitForSeconds(2f);
        regenDelay = true;

    }
    public void Update()
    {    
        RegenerateMana();
    }
    public void SpendMana(int manaCost)
    {
        if (manaPool >= manaCost)
        {
            manaPool -= manaCost;
            StartCoroutine(Delay());

        }
    }

    public float ManaNormalized()
    {
        return manaPool / maxMana;
    }
}