using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    public Dialogue[] dialogues;
    private bool isPlayerNearby = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log($"{npcName} detected player nearby.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log($"{npcName} player left.");
        }
    }

    public bool IsPlayerNearby()
    {
        return isPlayerNearby;
    }
}