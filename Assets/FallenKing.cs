using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject auraPrefab; // Le prefab de l'aura
    public Transform auraSpawnPoint; // Le point de spawn de l'aura
    public Transform player; // Référence au joueur
    public float detectionRange = 15f; // Portée de détection pour attaquePatte
    public float smashRange = 5f; // Portée de détection pour Smash
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthThreshold = 50f; // Seuil de points de vie pour lancer l'aura
    public float auraDuration = 5f; // Durée de l'aura
    public float auraRegenRate = 10f; // Taux de régénération de l'aura
    public float walkSpeed = 3f; // Vitesse de déplacement

    private Animator animator;
    private bool isAuraActive = false;
    private bool hasAuraBeenUsed = false; // Nouvelle variable pour vérifier si l'aura a été utilisée

    private GameObject currentAuraInstance; // Référence à l'instance actuelle de l'aura

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
    }

    private void Update()
    {
        if (player == null) return;

        if (currentHealth <= healthThreshold && !isAuraActive && !hasAuraBeenUsed)
        {
            TriggerAura();
        }

        // Initialisation du mouvement si nécessaire
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Walk");
        }
    }

    private void TriggerAura()
    {
        isAuraActive = true;
        hasAuraBeenUsed = true; // Marquer l'aura comme utilisée
        animator.SetTrigger("Aura");
    }

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

    public void DestroyAura()
    {
        if (currentAuraInstance != null)
        {
            Destroy(currentAuraInstance);
            Debug.Log("Aura détruite");
        }
        isAuraActive = false;
    }

    // Dessiner les gizmos pour la portée de détection et la portée de smash
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smashRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}