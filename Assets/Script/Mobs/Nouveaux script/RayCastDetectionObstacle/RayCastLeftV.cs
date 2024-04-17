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
                Debug.DrawLine(transform.position, hit.point, Color.yellow);
                transform.parent.position += Vector3.down * 0.01f;
            }
            else
            {
                Vector3 EndRaycast = transform.position + Vector3.left;
                Debug.DrawLine(transform.position, EndRaycast, Color.red);
            }
        }
    }
}
