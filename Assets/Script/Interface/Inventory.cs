using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [Header("Mana")]
    private int manaPotAmount;
    [HideInInspector] public int maxManaPot = 5;
    private bool drinkMana;
    private bool addManaPot;
    private Mana mana;
    [SerializeField] private TextMeshProUGUI manaTextMeshPro;

    [Header("Life")]
    private int lifePotAmount;
    [HideInInspector] public int maxLifePot = 5;
    private bool drinkLife;
    private bool addLifePot;
    private PlayerLifeSystem playerLifeSystem;
    [SerializeField] private TextMeshProUGUI lifeTextMeshPro;

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
        if (Input.GetKeyDown(KeyCode.E) && manaPotAmount > 0 && mana.manaPool < 100)
        {
            drinkMana = true;
        }
        if (Input.GetKeyDown(KeyCode.F) && lifePotAmount > 0 && playerLifeSystem.lifePool < 100)
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
        lifeTextMeshPro.text = lifePotAmount.ToString();
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
        if (manaPotAmount < maxManaPot)
        {
            manaPotAmount++;
            addManaPot = false;
        }
        else if (manaPotAmount == maxManaPot)
        {
            addManaPot = false;
        }
    }

    public void AddLifePotion()
    {
        if (lifePotAmount < maxLifePot)
        {
            lifePotAmount++;
            addLifePot = false;
        }
        else if (lifePotAmount == maxLifePot)
        {
            addLifePot = false;
        }
    }

    void DrinkManaPotion()
    {
        mana.manaPool = 100;
        manaPotAmount--;
        drinkMana = false;
    }

    void DrinkLifePotion()
    {
        playerLifeSystem.lifePool = 100;
        lifePotAmount--;
        drinkLife = false;
    }
}