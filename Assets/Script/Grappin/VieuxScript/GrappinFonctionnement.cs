using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappinFonctionnement : MonoBehaviour
{
    [SerializeField] private float longueurGrappin;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LineRenderer corde;
    private Vector3 PointDAccroche;
    private DistanceJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        corde.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            RaycastHit2D hit = Physics2D.Raycast
                (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layer);
            if (hit.collider != null) 
            {
                PointDAccroche = hit.point;
                PointDAccroche.z = 0f;
                joint.connectedAnchor = PointDAccroche;
                joint.enabled = true;
                joint.distance = longueurGrappin;
                corde.SetPosition(0, PointDAccroche);
                corde.SetPosition (1, transform.position);
                corde.enabled = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            corde.enabled = false;
        }
        if (corde.enabled == true) 
        {
            corde.SetPosition(1, transform.position);
        }
    }
}
