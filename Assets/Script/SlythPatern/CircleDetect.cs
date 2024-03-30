using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Collider2D Bited;

    private void OnEnable() // A chaque r�activation ...
    {
        StartCoroutine(ReactionTime()); // D�marre une coroutine qui va regarder si le joueur est encore � port�
    }
    private void OnDisable()// A chaque d�sactivation ....
    {
        Bited.enabled = false; // ... D�sactive le collider 
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
