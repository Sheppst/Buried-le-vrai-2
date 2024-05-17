using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeSystem : MonoBehaviour
{
    public float lifePool = 100;
    [HideInInspector] public const float maxLife = 100f;

    private Vector3 lastCheckpointPosition;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb2D;

    void Start()
    {
        lastCheckpointPosition = transform.position;
        playerMovement = GetComponent<PlayerMovement>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (lifePool <= 0f)
        {
            SceneManager.LoadScene(0);
        }
    }

    public float LifeNormalized()
    {
        return lifePool / maxLife;
    }

    public void TakeDamage(float amount)
    {
        lifePool -= amount;
        if (lifePool <= 0f)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            UpdateCheckpoint(collision.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "pics")
        {
            LoseHealthAndRespawn();
        }
    }

    private void LoseHealthAndRespawn()
    {
        lifePool -= maxLife * 0.25f;

        if (lifePool <= 0f)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            transform.position = lastCheckpointPosition;
            rb2D.velocity = Vector2.zero; // Reset the player's velocity
            StartCoroutine(RespawnDelay());
        }
    }

    private void UpdateCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    private IEnumerator RespawnDelay()
    {
        playerMovement.canControl = false;
        playerMovement.SetAnimationsEnabled(false);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;
        float blinkTime = 0.1f;

        while (elapsedTime < 1f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkTime);
            elapsedTime += blinkTime;
        }

        spriteRenderer.enabled = true;
        playerMovement.SetAnimationsEnabled(true);
        playerMovement.canControl = true;
    }
}