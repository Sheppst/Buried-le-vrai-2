using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpChamp : MonoBehaviour
{
    [SerializeField] private float Bounce = 20f;
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
        
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Bounce, ForceMode2D.Impulse);
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
