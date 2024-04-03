using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGround : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.5f);
        if (hit.point == null && hit.collider.gameObject.tag != "Player")
        {
            transform.parent.position += Vector3.down * 0.01f;
        }
    }
}
