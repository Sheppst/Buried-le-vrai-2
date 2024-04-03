using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBite : MonoBehaviour
{
    [SerializeField] private LayerMask layer; // Layers de détection
    [SerializeField] private GameObject Bite; // Objet lançant la morsure
    private Transform current;
    public bool Detect;
    private bool Desactivate;

    // Update is called once per frame
    private void OnEnable() // A l'activation de l'objet // Priorite 1
    {
        Desactivate = false;
        current = GetComponentInParent<Phase01>().Current; // Recupère l'objet servant d'objectif au boss
        RaycastHit2D AttackRange = Physics2D.Raycast(transform.position, transform.right, 1.4f, layer); // Crée un raycast allant d'un point A à un point B sur 2 pixels dans layer déclarer
        if (AttackRange.collider != null) // Si le raycast détecte qlq chose
        {
            Bite.SetActive(true); // Active l'objet de morsure 
            Debug.DrawLine(transform.position, AttackRange.point, Color.yellow); //Dessine un trait jaune en fonction du point A et du point de collision
            Detect = true;
        }
        else
        {
            Bite.SetActive(false); //Désactive l'objet de morsure
            Debug.DrawLine(transform.position, new Vector3(current.position.x, transform.position.y, transform.position.z), Color.red); //Dessine le raycast en rouge
        }
    }
    void Update() // Priorite 4
    {
        current = GetComponentInParent<Phase01>().Current;
        Vector3 lastpos = GetComponentInParent<Phase01>().PositionJoueur();
        RaycastHit2D AttackRange = Physics2D.Raycast(transform.position, transform.right, 2, layer); // Crée un raycast allant d'un point A à un point B sur 2 pixels dans layer déclarer
        if (AttackRange.collider != null && !Desactivate) // Si le raycast détecte qlq chose
        {
            Bite.SetActive(true); // Active l'objet de morsure 
            Debug.DrawLine(transform.position, AttackRange.point, Color.yellow); //Dessine un trait jaune en fonction du point A et du point de collision
            Detect = true;
        }
        else
        {
            Debug.DrawLine(transform.position, new Vector3 (current.position.x,transform.position.y,transform.position.z) , Color.red); //Dessine le raycast en rouge
        }
        if (lastpos.x == transform.position.x || Desactivate) // Si l'objet est à la même position que la dernière position du joueur ou que la morsure à été effectuer
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        Bite.SetActive(false); //Désactive l'objet de morsure
        Detect = false;
        Desactivate = false;
    }
    public void Desactivation()
    {
        Desactivate = true;
    }
    
}
