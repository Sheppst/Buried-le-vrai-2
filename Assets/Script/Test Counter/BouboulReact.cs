using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouboulReact : MonoBehaviour
{
    private float speed = 0.005f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed;
        if (transform.position.x >= 10)
        { Destroy(gameObject); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Block")
        { speed = -speed; }
    }
}
