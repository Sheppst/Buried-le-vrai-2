using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBite : MonoBehaviour
{
    [SerializeField] private LayerMask layer; // Layers de d�tection
    [SerializeField] private GameObject Bite; // Objet lan�ant la morsure
    private Vector3 current;
    public bool Detect;
    public bool Pass;

    // Update is called once per frame
    private void OnEnable() // A l'activation de l'objet
    {
        current = GetComponentInParent<Phase01>().Newpos; // Recup�re l'objet servant d'objectif au boss
        Bite.SetActive(false); //D�sactive l'objet de morsure
    }
    void Update()
    {
        current = GetComponentInParent<Phase01>().Newpos;
        RaycastHit2D AttackRange = Physics2D.Raycast(transform.position, transform.right, 2, layer); // Cr�e un raycast allant d'un point A � un point B sur 2 pixels dans layer d�clarer
        if (AttackRange.collider != null) // Si le raycast d�tecte qlq chose
        {
            Bite.SetActive(true); // Active l'objet de morsure 
            Debug.DrawLine(transform.position, AttackRange.point, Color.yellow); //Dessine un trait jaune en fonction du point A et du point de collision
            Detect = true;
        }
        else
        {
            Debug.DrawLine(transform.position, new Vector3 (current.x,transform.position.y,transform.position.z) , Color.red); //Dessine le raycast en rouge
        }
        if (current.x == transform.position.x || Pass)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        Bite.SetActive(false); //D�sactive l'objet de morsure
        Detect = false;
    }
    
}
