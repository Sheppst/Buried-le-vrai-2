using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interface;

public class Human : MonoBehaviour, Interactable
{
    private int Life = 5;
    private void Update()
    {
        if (Life <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void InteractOwn()
    {
        Life -= 1;
    }
}
