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
    [SerializeField] private float shakeDuration = 0.2f; // Durée du tremblement de la caméra
    [SerializeField] private float shakeMagnitude = 0.3f; // Intensité du tremblement de la caméra
    [SerializeField] private float PropulseX;
    [SerializeField] private float PropulseY;
    [SerializeField] private float ChargePower;

    [SerializeField] private GameObject Boss;

    private Vector3 lastCheckpointPosition;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb2D;
    private Coroutine auraDamageCoroutine;
    private bool isInvincible = false;

    [SerializeField] private Transform bossTransform;
    private CharacterController2D controller;
    private CameraShake cameraShake; // Référence au script CameraShake

    void Start()
    {
        lastCheckpointPosition = transform.position;
        playerMovement = GetComponent<PlayerMovement>();
        rb2D = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Obtenez la référence au script CameraShake
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
            lifePool -= 3f;
            if (lifePool <= 0f)
            {
                SceneManager.LoadScene(0);
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
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

        StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude, transform.position)); // Appeler le tremblement de la caméra

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
        // Déterminer la direction du knockback en fonction de la position relative du joueur et du boss
        if (bossTransform.position.x < transform.position.x)
        {
            return Vector2.right; // Boss est à gauche du joueur, donc propulser vers la droite
        }
        else
        {
            return Vector2.left; // Boss est à droite du joueur, donc propulser vers la gauche
        }
    }

    public void BiteByBoss()
    {
        print("Mordu");
        if (Boss.GetComponent<Phase01>().transform.localScale.x < 0)
        {
            rb2D.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * -1, PropulseY);
            rb2D.AddForce(Repouss);
        }
        else if (Boss.GetComponent<Phase01>().transform.localScale.x > 0)
        {
            rb2D.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX, PropulseY);
            rb2D.AddForce(Repouss);
        }
        lifePool -= 20;
    }

    public void ChargeByBoss()
    {
        print("Chargé");
        if (Boss.GetComponent<Phase01>().transform.localScale.x < 0)
        {
            rb2D.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * -1 * ChargePower, PropulseY * ChargePower);
            rb2D.AddForce(Repouss);
        }
        else if (Boss.GetComponent<Phase01>().transform.localScale.x > 0)
        {
            rb2D.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * ChargePower, PropulseY * ChargePower);
            rb2D.AddForce(Repouss);
        }
        lifePool -= 15;
    }
}
