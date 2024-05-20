using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeSystem : MonoBehaviour
{
    public float lifePool = 100;
    [HideInInspector] public const float maxLife = 100f;

    [SerializeField] private float invincibilityDuration = 1f; // Durée d'invincibilité en secondes, réglable dans l'Inspector
    [SerializeField] private float blinkTime = 0.2f; // Durée de clignotement en secondes, réglable dans l'Inspector
    [SerializeField] private float smashHorizontalForce = 10f; // Force horizontale pour le smash
    [SerializeField] private float smashVerticalForce = 5f; // Force verticale pour le smash
    [SerializeField] private float smashDuration = 1f; // Durée pendant laquelle la force horizontale est appliquée

    private Vector3 lastCheckpointPosition;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb2D;
    private Coroutine auraDamageCoroutine;
    private bool isInvincible = false;

    [SerializeField] private Transform bossTransform;
    private CharacterController2D controller;

    void Start()
    {
        lastCheckpointPosition = transform.position;
        playerMovement = GetComponent<PlayerMovement>();
        rb2D = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
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
        if (!isInvincible)
        {
            lifePool -= amount;
            if (lifePool <= 0f)
            {
                SceneManager.LoadScene(0);
            }
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void TakeDamageWithKnockback(float amount, Vector2 direction)
    {
        if (!isInvincible)
        {
            lifePool -= amount;
            if (lifePool <= 0f)
            {
                SceneManager.LoadScene(0);
            }
            StartCoroutine(ApplyKnockback(direction));
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            UpdateCheckpoint(collision.transform.position);
        }
        else if (collision.tag == "Smash")
        {
            Vector2 direction = DetermineKnockbackDirection();
            TakeDamageWithKnockback(40f, direction);
        }
        else if (collision.tag == "Aura")
        {
            if (auraDamageCoroutine == null)
            {
                auraDamageCoroutine = StartCoroutine(TakeAuraDamage());
            }
        }
        else if (collision.tag == "attaquePattes")
        {
            TakeDamage(30f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Aura" && auraDamageCoroutine != null)
        {
            StopCoroutine(auraDamageCoroutine);
            auraDamageCoroutine = null;
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
            rb2D.velocity = Vector2.zero;
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

    private IEnumerator TakeAuraDamage()
    {
        while (true)
        {
            TakeDamage(10f);
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkTime);
            elapsedTime += blinkTime;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    private IEnumerator ApplyKnockback(Vector2 direction)
    {
        float elapsedTime = 0f;
        controller.isKnockedBack = true; // Désactiver le mouvement pendant la propulsion
        rb2D.velocity = Vector2.zero; // Réinitialiser la vélocité

        rb2D.AddForce(new Vector2(direction.x * smashHorizontalForce, smashVerticalForce), ForceMode2D.Impulse);
        playerMovement.SetAnimationsEnabled(false); // Désactiver les animations

        while (elapsedTime < smashDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        controller.isKnockedBack = false; // Réactiver le mouvement après la propulsion
        playerMovement.SetAnimationsEnabled(true); // Réactiver les animations
    }

    private Vector2 DetermineKnockbackDirection()
    {
        Vector2 direction;
        if (bossTransform.localScale.x > 0)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
        return direction;
    }
}

