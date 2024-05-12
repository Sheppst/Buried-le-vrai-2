using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Inventory : MonoBehaviour
{
    [Header("Mana")]
    public int manaPotAmount;
    [HideInInspector] public int maxManaPot =5;
    private bool drinkMana;
    private Mana mana;
    public TextMeshProUGUI manaTextMeshPro;
    [Header("Life")]
    public int lifePotAmount;
    [HideInInspector] public int maxLifePot = 5;
    private bool drinkLife;
    private PlayerLifeSystem playerLifeSystem;
    public TextMeshProUGUI lifeTextMeshPro;
    


    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        mana = player.GetComponent<Mana>();
        playerLifeSystem = player.GetComponent<PlayerLifeSystem>();
        manaPotAmount = 1;
        lifePotAmount = 1;


    }	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && manaPotAmount > 0)
        {
            drinkMana = true;
        }
        if (drinkMana) 
        {
            DrinkManaPotion();
        }
        if (Input.GetKeyDown(KeyCode.F) && lifePotAmount > 0)
        {
            drinkLife = true;
        }
        if (drinkLife)
        {
            DrinkLifePotion();
        }
        lifeTextMeshPro.text =  lifePotAmount.ToString();
        manaTextMeshPro.text = manaPotAmount.ToString();
    }

    void DrinkManaPotion()
    {        
        mana.manaPool = 100;
         manaPotAmount = manaPotAmount - 1;
          drinkMana = false;     
    }
    void DrinkLifePotion()
    {
        playerLifeSystem.lifePool = 100;
        lifePotAmount = lifePotAmount - 1;
        drinkLife = false;
    }
}
