using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplinGun : MonoBehaviour
{
    private float speed = 1; 
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += Vector3.RotateTowards(transform.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position, Time.deltaTime * speed, 0);
    }
}
