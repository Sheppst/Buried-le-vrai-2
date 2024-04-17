using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastLeftV : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    [SerializeField] GameObject[] Detects;
    public bool Change;
    // Update is called once per frame
    void Update()
    {
        if (!Change)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, layer);
            if (hit.point != null && hit.collider.gameObject.tag != "Player")
            {
                Detects[0].GetComponent<RayCastRightV>().Change=true;
                Detects[1].GetComponent<RayCastRooffed>().Change = true;
                Detects[2].GetComponent<RayCastRooffed>().Change = true;
                Debug.DrawLine(transform.position, hit.point, Color.yellow);
                transform.parent.position += Vector3.down * 0.01f;
            }
            else
            {
                Detects[0].GetComponent<RayCastRightV>().Change = false;
                Detects[1].GetComponent<RayCastRooffed>().Change = false;
                Detects[2].GetComponent<RayCastRooffed>().Change = false;
                Vector3 EndRaycast = transform.position + Vector3.left;
                Debug.DrawLine(transform.position, EndRaycast, Color.red);
            }
        }
    }
}
