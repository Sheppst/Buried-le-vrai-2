using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatform : MonoBehaviour
{
    // Elliot Script
    [SerializeField] private float FallDelay;
    [SerializeField] float DestroyDelay;
    [SerializeField] float gravity;
    [SerializeField] private Rigidbody2D rb;

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(FallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, DestroyDelay);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           StartCoroutine(Fall());

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
