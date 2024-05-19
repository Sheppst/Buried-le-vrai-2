using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject auraPrefab; // Le prefab de l'aura
    public Transform auraSpawnPoint; // Le point de spawn de l'aura
    public GameObject attackPrefab; // Le prefab pour l'attaque Pattes
    public Transform player; // Référence au joueur
    public float detectionRange = 15f; // Portée de détection pour attaquePatte
    public float smashRange = 5f; // Portée de détection pour Smash
    public float maxHealth = 100f;
    public float healthThreshold = 50f; // Seuil de points de vie pour lancer l'aura
    public float auraDuration = 5f; // Durée de l'aura
    public float auraRegenRate = 10f; // Taux de régénération de l'aura
    public float attackInterval = 0.4f; // Intervalle entre les apparitions des prefabs de l'attaque Pattes
    public float attackCooldown = 2f; // Temps de recharge entre les attaques
    public LayerMask groundLayer; // Le layer mask pour le sol

    private Animator animator;
    private float currentHealth;
    private bool isAuraActive = false;
    private bool hasAuraBeenUsed = false; // Nouvelle variable pour vérifier si l'aura a été utilisée
    private float lastAttackTime;

    private GameObject currentAuraInstance; // Référence à l'instance actuelle de l'aura
    private Coroutine attackCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (auraPrefab == null)
        {
            Debug.LogError("auraPrefab n'est pas assigné !");
        }

        if (auraSpawnPoint == null)
        {
            Debug.LogError("auraSpawnPoint n'est pas assigné !");
        }

        if (player == null)
        {
            Debug.LogError("player n'est pas assigné !");
        }

        if (attackPrefab == null)
        {
            Debug.LogError("attackPrefab n'est pas assigné !");
        }

        Debug.Log("Initialisation des valeurs terminée.");
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distanceToPlayer);

        if (currentHealth <= healthThreshold && !isAuraActive && !hasAuraBeenUsed)
        {
            Debug.Log("TriggerAura conditions met.");
            TriggerAura();
        }
        else if (!isAuraActive)
        {
            if (distanceToPlayer <= smashRange && Time.time > lastAttackTime + attackCooldown)
            {
                Debug.Log("TriggerSmash conditions met.");
                TriggerSmash();
            }
            else if (distanceToPlayer <= detectionRange && Time.time > lastAttackTime + attackCooldown)
            {
                Debug.Log("TriggerAttaquePatte conditions met.");
                TriggerAttaquePatte();
            }
        }
    }

    private void TriggerAura()
    {
        if (animator == null)
        {
            Debug.LogError("Animator n'est pas assigné !");
            return;
        }

        isAuraActive = true;
        hasAuraBeenUsed = true; // Marquer l'aura comme utilisée
        animator.ResetTrigger("Smash");
        animator.ResetTrigger("AttaquePattes");
        animator.SetTrigger("Aura");
        SpawnAura();
        StartCoroutine(AuraEffect());
    }

    private void TriggerSmash()
    {
        if (animator == null)
        {
            Debug.LogError("Animator n'est pas assigné !");
            return;
        }

        lastAttackTime = Time.time;
        animator.SetTrigger("Smash");
    }

    private void TriggerAttaquePatte()
    {
        if (animator == null)
        {
            Debug.LogError("Animator n'est pas assigné !");
            return;
        }

        lastAttackTime = Time.time;
        animator.SetTrigger("AttaquePattes");

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(SpawnAttackPrefabs());
    }

    // Coroutine pour instancier les prefabs de l'attaque Pattes
    private IEnumerator SpawnAttackPrefabs()
    {
        Debug.Log("Commence à instancier les prefabs d'attaque Pattes.");
        while (true)
        {
            // Vérifiez si l'animation est toujours en cours
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("AttaquePattes"))
            {
                Debug.Log("Animation 'AttaquePattes' terminée.");
                break;
            }

            Vector3 spawnPosition = GetGroundPosition(player.position);
            Debug.Log("Instanciation du prefab d'attaque Pattes à la position : " + spawnPosition);
            Instantiate(attackPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(attackInterval);
        }
        Debug.Log("Arrêt de l'instanciation des prefabs d'attaque Pattes.");
    }

    // Méthode pour obtenir la position au sol sous le joueur
    private Vector3 GetGroundPosition(Vector3 playerPosition)
    {
        RaycastHit hit;
        // Lancer un raycast vers le bas depuis la position du joueur
        if (Physics.Raycast(playerPosition, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Debug.Log("Sol détecté à la position : " + hit.point);
            return hit.point; // Retourner le point de contact avec le sol
        }
        else
        {
            Debug.LogError("Sol non détecté sous le joueur");
            return playerPosition; // Si aucun sol n'est détecté, retourner la position actuelle du joueur
        }
    }

    // Méthode pour instancier l'aura
    public void SpawnAura()
    {
        if (auraPrefab != null && auraSpawnPoint != null)
        {
            currentAuraInstance = Instantiate(auraPrefab, auraSpawnPoint.position, auraSpawnPoint.rotation);
            Debug.Log("Aura instanciée");
        }
        else
        {
            Debug.LogError("Échec de l'instanciation de l'aura : le prefab ou le point de spawn est null");
        }
    }

    // Coroutine pour gérer les effets de l'aura
    private IEnumerator AuraEffect()
    {
        float endTime = Time.time + auraDuration;
        while (Time.time < endTime)
        {
            currentHealth = Mathf.Min(currentHealth + auraRegenRate * Time.deltaTime, maxHealth);
            yield return null;
        }
        isAuraActive = false;

        if (currentAuraInstance != null)
        {
            Destroy(currentAuraInstance);
            Debug.Log("Aura détruite");
        }
    }
}
