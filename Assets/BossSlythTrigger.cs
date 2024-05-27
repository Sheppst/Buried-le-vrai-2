using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlythTrigger : MonoBehaviour
{

    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject Wall;

    private void Awake()
    {
        Wall.SetActive(false);
        Boss.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Wall.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Boss.SetActive(true);
            this.enabled = false;
        }
    }
}
