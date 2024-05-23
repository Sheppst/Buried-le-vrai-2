using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    [SerializeField] private LayerMask layer; //Layer de d�tectection
    [SerializeField] private Animator BossAnimator;
    //[SerializeField] private Collider2D Bited; // Collider de l'objet

    private void OnEnable() // A chaque r�activation ...
    {
        BossAnimator.SetTrigger("Bite");
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.5f, layer); // Lance le cercle de d�tection de port�e de la morsure                                                                       // autrement il sera "false"
        if (collision != null) //Si oui...
        {
            //Bited.enabled = true; // ... Active le collider d'attaque (Equivalent de la morsure
            GameObject.Find("Player").GetComponent<PlayerMovement>().BiteByBoss();GameObject.Find("DetectRay").GetComponent<DetectBite>().Desactivation();
        }
        gameObject.SetActive(false); // D�sactive l'objet
         // � remplacer par une fonction // PROBLEME : d�sactive et r�active trop rapidement � cause de �a donc non XD


    }
    private void OnDisable()// A chaque d�sactivation ....
    {
        //Bited.enabled = false; // ... D�sactive le collider
    }
}
