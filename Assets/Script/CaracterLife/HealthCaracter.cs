using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCaracter : MonoBehaviour
{
    public int maxhealth = 3;
    public int currentHealth;

    public HealBar healthBar;
    void Start()
    {
        currentHealth = maxhealth;
        healthBar.SetHealth(maxhealth);
    }
}
