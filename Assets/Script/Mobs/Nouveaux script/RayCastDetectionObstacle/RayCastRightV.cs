using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastRightV : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, layer);
        if (hit.point != null && hit.collider.gameObject.tag != "Player")
        {
            Debug.DrawLine(transform.position, hit.point,Color.yellow);
            transform.parent.position += Vector3.down * 0.01f;
        }
        else
        {
            Debug.DrawLine(transform.position,transform.right,Color.red);
        }
    }
}
