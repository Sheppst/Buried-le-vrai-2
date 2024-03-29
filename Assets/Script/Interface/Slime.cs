using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interface;

public class Slime : MonoBehaviour , Interactable
{
    private int Life = 3;
    private void Update()
    {
        transform.localScale = new Vector3(Life, Life, Life);
    }
    public void InteractOwn()
    {
        Life -= 1;
    }
}
