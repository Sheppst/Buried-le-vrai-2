using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Collider2D Bited;

    private void OnEnable() // A chaque réactivation ...
    {
        StartCoroutine(ReactionTime()); // Démarre une coroutine qui va regarder si le joueur est encore à porté
    }
    private void OnDisable()// A chaque désactivation ....
    {
        Bited.enabled = false; // ... Désactive le collider 
        StopAllCoroutines(); //... Stop toutes  coroutine 
    }
    private IEnumerator ReactionTime() 
    {
        yield return new WaitForSeconds(0.5f); // Temps d'attente
        bool Lock = GameObject.Find("DetectRay").GetComponent<DetectBite>().Lock;
        if (Lock)
        {
            Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.5f, layer);
            if (IsBited(collision))
            {
                Bited.enabled = true;
            }
        }
        gameObject.SetActive(false);
    }
    private bool IsBited(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            return true;
        }
        return false;
    }
}
