using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastRight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1);
        if (hit.point == null)
        {
            transform.parent.position += Vector3.up * 0.01f;
        }
    }
}
