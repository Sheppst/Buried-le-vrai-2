using Balise_SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

// Crée une ligne sur entre le joueur et une balise envoyé créant un grappin 

public class SM_GrappinFonctionnementOwn : MonoBehaviour
{
    [SerializeField] private float longueurGrappin;
    [SerializeField] private LineRenderer corde;
    [SerializeField] private GameObject balise;
    [SerializeField] private bool Debug0;
    private Vector3 PointDAccroche;
    private DistanceJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>(); //Je récupère un cpmnt de DistJoint2D et le range dans joint
        joint.enabled = false; // Je désactive joint
        corde.enabled = false; // Je désactive le LineRenderer "corde"
    }

    // Update is called once per frame
    void Update()
    {
        PointDAccroche = balise.transform.position; // La variable PtD'accroche sera toujours égal à la balise
        if (Input.GetMouseButtonDown(0)) // Si [Souris Gauche] est presser...
        {
            corde.enabled = true; // Active le LineRenderer
            corde.SetPosition(0, PointDAccroche); // La position en 0 est celle qui prend la position de la balise
            corde.SetPosition(1, transform.position); // La positon en 1 restera en permanence sur le joueur
        }
        if (balise.GetComponent<SM_LaunchBalise>().ReturnState("Contact"))
        {
            joint.connectedAnchor = PointDAccroche; // Le connectAncor va accroché le joint à la position donné, ici c'est la position de la balise lors du contact
            joint.enabled = true; // Active le joint après avoir paramêtrer son point d'accroche 
            joint.distance = longueurGrappin; // Distance entre la position de la position 0 (point d'accroche) et de la position 1 (Joueur)
        }
        if (Input.GetMouseButtonUp(0) || balise.GetComponent<SM_LaunchBalise>().ReturnState("Inactive")) // Si le boutton d'activation est réactivé ou si la balise n'est plus active
        {
            joint.enabled = false; // désactive le joint
        }
        if (balise.transform.position == transform.position) // Si la balise est revenu
        {
            corde.enabled = false; // Désactive le LineRenderer
        }
        if (corde.enabled == true) // Si la corde est encore active l'actualise
        {
            corde.SetPosition(0, PointDAccroche);
            corde.SetPosition(1, transform.position);
        }
        
    }
}
