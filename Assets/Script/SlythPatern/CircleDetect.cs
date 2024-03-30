using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    [SerializeField] private LayerMask layer; //Layer de d�tectection
    [SerializeField] private Collider2D Bited; // Collider de l'objet

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
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.5f, layer); // Lance le cercle de d�tection de port�e de la morsure                                                                       // autrement il sera "false"
        if (collision != null) //Si oui...
        {
            Bited.enabled = true; // ... Active le collider d'attaque (Equivalent de la morsure
        }
        gameObject.SetActive(false); // D�sactive l'objet
    }
}
