using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float speed;
    [Header("Check")]
    private bool IsGrounded;

    void Start()
    {

    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
       
        if (Mathf.Abs(xInput) > 0)
        {
            body.velocity = new Vector2(xInput * speed, body.velocity.y); 
        }
        if (Mathf.Abs(yInput) > 0)
        {
            body.velocity = new Vector2(body.velocity.x, yInput*speed);
        }
    }
}