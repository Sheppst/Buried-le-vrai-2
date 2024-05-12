using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Inventory : MonoBehaviour
{
    [Header("Mana")]
    public int manaPotAmount;
    [HideInInspector] public int maxManaPot = 5;
    private bool drinkMana;
    public bool addManaPot;
    private Mana mana;
    public TextMeshProUGUI manaTextMeshPro;


    [Header("Life")]
    public int lifePotAmount;
    [HideInInspector] public int maxLifePot = 5;
    private bool drinkLife;
    public bool addLifePot;
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
        //Input
        if (Input.GetKeyDown(KeyCode.E) && manaPotAmount > 0)
        {
            drinkMana = true;
        }
        if (Input.GetKeyDown(KeyCode.F) && lifePotAmount > 0)
        {
            drinkLife = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            addManaPot = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            addLifePot = true;
        }
        //Drink
        if (drinkMana)
        {
            DrinkManaPotion();
        }
        if (drinkLife)
        {
            DrinkLifePotion();
        }
        //Text
        lifeTextMeshPro.text =  lifePotAmount.ToString();
        manaTextMeshPro.text = manaPotAmount.ToString();

        //AddPotion
        if (addManaPot)
        {
            AddManaPotion();
        }

        if (addLifePot)
        {
            AddLifePotion();
        }
       
    }
    
    public void AddManaPotion()
    {
        if ( manaPotAmount < 5)
        {
            manaPotAmount = manaPotAmount + 1;
            addManaPot = false;
        }
        else if (manaPotAmount == 5)
        {
            addManaPot = false;
        }
        
    }
    public void AddLifePotion()
    {
        if (lifePotAmount < 5)
        {
            lifePotAmount = lifePotAmount + 1;
            addLifePot = false;
        }
        else if (lifePotAmount == 5)
        {
            addLifePot = false;
        }

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
