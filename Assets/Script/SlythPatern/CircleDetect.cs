using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    [SerializeField] private LayerMask layer; //Layer de détectection
    [SerializeField] private Collider2D Bited; // Collider de l'objet

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
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.5f, layer); // Lance le cercle de détection de portée de la morsure                                                                       // autrement il sera "false"
        if (collision != null) //Si oui...
        {
            Bited.enabled = true; // ... Active le collider d'attaque (Equivalent de la morsure
        }
        gameObject.SetActive(false); // Désactive l'objet
    }
}
