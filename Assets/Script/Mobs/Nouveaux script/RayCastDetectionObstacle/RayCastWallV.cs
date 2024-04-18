using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWallV : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    [SerializeField] GameObject Detects;
    public bool Change;
    // Update is called once per frame
    void Update()
    {
        if (transform.parent.localScale.x < 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (transform.parent.localScale.x > 0)
        {
            transform.eulerAngles = Vector3.right * 180;
        }
        if (!Change)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, layer);
            if (hit.point != null && hit.collider.gameObject.tag != "Player")
            {
                Detects.GetComponent<RayCastWallV>().Change = true;
                Debug.DrawLine(transform.position, hit.point, Color.yellow);
                transform.parent.position += Vector3.down * 0.01f;
            }
            else
            {
                Detects.GetComponent<RayCastWallV>().Change = false;
                Vector3 EndRaycast = transform.position + Vector3.left;
                Debug.DrawLine(transform.position, EndRaycast, Color.red);
            }
        }
    }
}
