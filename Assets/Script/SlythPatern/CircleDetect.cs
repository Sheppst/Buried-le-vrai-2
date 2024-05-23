using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    [SerializeField] private LayerMask layer; //Layer de détectection
    [SerializeField] private Animator BossAnimator;
    //[SerializeField] private Collider2D Bited; // Collider de l'objet

    private void OnEnable() // A chaque réactivation ...
    {
        BossAnimator.SetTrigger("Bite");
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.5f, layer); // Lance le cercle de détection de portée de la morsure                                                                       // autrement il sera "false"
        if (collision != null) //Si oui...
        {
            //Bited.enabled = true; // ... Active le collider d'attaque (Equivalent de la morsure
            GameObject.Find("Player").GetComponent<PlayerMovement>().BiteByBoss();GameObject.Find("DetectRay").GetComponent<DetectBite>().Desactivation();
        }
        gameObject.SetActive(false); // Désactive l'objet
         // à remplacer par une fonction // PROBLEME : désactive et réactive trop rapidement à cause de ça donc non XD


    }
    private void OnDisable()// A chaque désactivation ....
    {
        //Bited.enabled = false; // ... Désactive le collider
    }
}
