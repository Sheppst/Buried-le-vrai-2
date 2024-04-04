using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGround : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D Nohit = Physics2D.Raycast(transform.position, transform.up, 3f, layer);
        if (Nohit.point == null && Nohit.collider.gameObject.tag != "Player")
        {
            Vector3 EndRaycast = transform.position + Vector3.down * 3;
            Debug.DrawLine(transform.position, EndRaycast, Color.blue);
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.5f, layer);
            if (hit.point == null && hit.collider.gameObject.tag != "Player")
            {
                Vector3 EndRaycast = transform.position + Vector3.down * 0.5f;
                Debug.DrawLine(transform.position, EndRaycast , Color.yellow);
                transform.parent.position += Vector3.down * 0.01f;
            }
            else
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
        }
        
    }
}
