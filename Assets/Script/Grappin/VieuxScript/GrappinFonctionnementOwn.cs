using UnityEngine;

public class GrappinFonctionnementOwn : MonoBehaviour
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
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        corde.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        PointDAccroche = balise.transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            corde.enabled = true;
            corde.SetPosition(0, PointDAccroche); // Là il faut remplacer PointDAccroche par la position de la balise
            corde.SetPosition(1, transform.position);
            Debug0 = true;
        }
        if (balise.GetComponent<LaunchBalise>().Connect)
        {
            joint.connectedAnchor = PointDAccroche; // Là il faut remplacer PointDAccroche par le point de reception de la collision de la balise
            joint.enabled = true;
            joint.distance = longueurGrappin; // Distance entre la position de la position 0 et de la position 1
        }
        if (Input.GetMouseButtonUp(0) || balise.GetComponent<LaunchBalise>().Spawn)
        {
            joint.enabled = false;
            Debug0 = false;
        }
        if (corde.enabled == true)
        {
            corde.SetPosition(0, PointDAccroche);
            corde.SetPosition(1, transform.position);
        }
    }
}
