using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour
{
    public ParticleSystem dustPS; // Référence au Particle System de la poussière

    private CharacterController2D controller;
    private Rigidbody2D rb;
    private Vector2 lastVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckForDirectionChange();
    }

    public void CreateDust()
    {
        if (dustPS != null)
        {
            dustPS.Play();
        }
    }

    private void CheckForDirectionChange()
    {
        // Détecte un changement de direction horizontal
        if ((lastVelocity.x > 0 && rb.velocity.x < 0) || (lastVelocity.x < 0 && rb.velocity.x > 0))
        {
            CreateDust();
        }

        // Détecte un saut ou une chute
        if (controller.IsGrounded() && Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            CreateDust();
        }

        lastVelocity = rb.velocity;
    }
}
