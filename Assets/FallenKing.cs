using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour
{
    [Header("Aura Settings")]
    public GameObject auraPrefab; // Le prefab de l'aura
    public Transform auraSpawnPoint; // Le point de spawn de l'aura
    public float auraDuration = 5f; // Dur�e de l'aura
    public float auraRegenRate = 10f; // Taux de r�g�n�ration de l'aura

    [Header("Patte Settings")]
    public GameObject pattePrefab; // Le prefab des pattes
    public float groundYPosition = 0f; // Position Y du sol

    [Header("Detection Settings")]
    public Transform player; // R�f�rence au joueur
    public float detectionRange = 15f; // Port�e de d�tection pour attaquePatte
    public float smashRange = 5f; // Port�e de d�tection pour Smash

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthThreshold = 50f; // Seuil de points de vie pour lancer l'aura
    public float damageAmount = 10f; // Amount of damage taken per hit
    public float redIntensity = 1f; // Intensit� du rouge lors du flash
    public bool isDead = false; // Bool�en pour suivre l'�tat de vie du boss

    [Header("Movement Settings")]
    public float walkSpeed = 3f; // Vitesse de d�placement

    [Header("Gizmos Settings")]
    public bool drawGizmos = true; // Activer/d�sactiver les gizmos

    [Header("Collision Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius;

    [Header("Movement Boundaries")]
    public Collider2D movementBounds; // Zone de mouvement d�finie par un Collider2D

    [Header("Doors")]
    public List<DoorBehaviour> doors; // Liste des portes � ouvrir � la mort du boss

    private Animator animator;
    public bool isAuraActive = false;
    public bool hasAuraBeenUsed = false; // Nouvelle variable pour v�rifier si l'aura a �t� utilis�e
    private bool isInvincible = true; // Le boss est invincible au d�but
    private Coroutine patteCoroutine; // R�f�rence � la coroutine d'attaque pattes
    private Coroutine regenCoroutine; // R�f�rence � la coroutine de r�g�n�ration de l'aura
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool facingLeft = true;

    private bool isTouchingGround;
    private bool isTouchingWall;
    private Vector2 moveDirection = Vector2.left;

    public bool isAtLimit; // Variable pour v�rifier si le boss touche la limite

    public bool IsAtLimit => isAtLimit; // Propri�t� publique pour acc�der � isAtLimit

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        if (auraPrefab == null) Debug.LogError("auraPrefab n'est pas assign� !");
        if (pattePrefab == null) Debug.LogError("pattePrefab n'est pas assign� !");
        if (auraSpawnPoint == null) Debug.LogError("auraSpawnPoint n'est pas assign� !");
        if (player == null) Debug.LogError("player n'est pas assign� !");
        if (groundCheck == null) Debug.LogError("groundCheck n'est pas assign� !");
        if (wallCheck == null) Debug.LogError("wallCheck n'est pas assign� !");
        if (movementBounds == null) Debug.LogError("movementBounds n'est pas assign� !");

        // Le boss est invincible au d�but
        isInvincible = true;
    }

    private void Update()
    {
        if (player == null) return;

        if (isDead) return; // Ne rien faire si le boss est mort

        FlipTowardsPlayer();

        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (isTouchingWall)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            if (!isAtLimit)
            {
                rb.velocity = new Vector2(moveDirection.x * walkSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = Vector2.zero;
                animator.SetTrigger("Smash"); // D�clencher l'animation du smash
            }
        }

        // V�rifie et contraint la position du boss dans les limites d�finies
        ConstrainPositionWithinBounds();
    }

    private void ConstrainPositionWithinBounds()
    {
        if (movementBounds != null)
        {
            Vector3 bossPosition = transform.position;
            Vector3 minBounds = movementBounds.bounds.min;
            Vector3 maxBounds = movementBounds.bounds.max;

            bool wasAtLimit = isAtLimit;

            isAtLimit = bossPosition.x <= minBounds.x || bossPosition.x >= maxBounds.x;

            if (!wasAtLimit && isAtLimit)
            {
                // Le boss vient d'atteindre la limite, arr�ter l'animation de marche
                animator.SetBool("isWalking", false);
            }

            bossPosition.x = Mathf.Clamp(bossPosition.x, minBounds.x, maxBounds.x);
            bossPosition.y = Mathf.Clamp(bossPosition.y, minBounds.y, maxBounds.y);

            transform.position = bossPosition;
        }
    }

    public void StartAttaquePattes()
    {
        if (patteCoroutine == null)
        {
            patteCoroutine = StartCoroutine(AttaquePattesCoroutine());
        }
    }

    public void StopAttaquePattes()
    {
        if (patteCoroutine != null)
        {
            StopCoroutine(patteCoroutine);
            patteCoroutine = null;
        }
    }

    private IEnumerator AttaquePattesCoroutine()
    {
        while (true)
        {
            SpawnPattes();
            yield return new WaitForSeconds(0.3f); // Temps entre chaque instantiation, ajustez selon vos besoins
        }
    }

    private void SpawnPattes()
    {
        if (pattePrefab != null && player != null)
        {
            Vector3 spawnPosition = new Vector3(player.transform.position.x, groundYPosition, player.transform.position.z);
            Instantiate(pattePrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Patte instanci�e");
        }
        else
        {
            Debug.LogError("�chec de l'instanciation de la patte : le prefab ou le joueur est null");
        }
    }

    private void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection > 0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        moveDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    public void TakeDamage(float damageAmount)
    {
        if (isInvincible || isDead)
            return;

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (currentHealth <= maxHealth * 0.5f && !hasAuraBeenUsed)
        {
            isAuraActive = true;
            hasAuraBeenUsed = true;
            animator.SetTrigger("Aura"); // Active le trigger "Aura"
            StartCoroutine(RegenerateHealth());
        }

        StartCoroutine(FlashRed());
        StartCoroutine(TemporaryInvincibility(0.1f)); // Rendre le boss invincible pendant 0.1 seconde
    }

    private void Die()
    {
        isDead = true; // Le boss est maintenant mort
        animator.SetBool("isDead", true); // D�clencher l'animation de mort

        // Ouvrir les portes assign�es
        foreach (var door in doors)
        {
            door.isDoorOpen = true;
        }
    }

    private IEnumerator TemporaryInvincibility(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1f, 0f, 0f, redIntensity); // Intensit� du rouge ajustable
        yield return new WaitForSeconds(0.1f); // Duration of the flash
        spriteRenderer.color = originalColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjPlayer"))
        {
            TakeDamage(damageAmount);
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, smashRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);

            if (movementBounds != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(movementBounds.bounds.center, movementBounds.bounds.size);
            }
        }
    }

    private IEnumerator RegenerateHealth()
    {
        isInvincible = true; // Le boss devient invincible
        float timer = 0f;

        while (timer < auraDuration)
        {
            currentHealth += auraRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp currentHealth to maxHealth
            timer += Time.deltaTime;
            yield return null;
        }

        isInvincible = false; // Le boss redevient vuln�rable apr�s la r�g�n�ration
    }

    public void GoToIdleState()
    {
        isInvincible = false; // Le boss redevient vuln�rable
        animator.SetTrigger("EndDialogue");
    }
}

