using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPics : MonoBehaviour
{
    // Elliot Script
    [SerializeField] private float FallDelay;
    [SerializeField] float DestroyDelay;

    [SerializeField] private Rigidbody2D rb;

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(FallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        Destroy(gameObject, DestroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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