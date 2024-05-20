using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public GameObject auraPrefab; // Le prefab de l'aura
    public GameObject pattePrefab; // Le prefab des pattes
    public Transform auraSpawnPoint; // Le point de spawn de l'aura
    public Transform player; // R�f�rence au joueur
    public float detectionRange = 15f; // Port�e de d�tection pour attaquePatte
    public float smashRange = 5f; // Port�e de d�tection pour Smash
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthThreshold = 50f; // Seuil de points de vie pour lancer l'aura
    public float auraDuration = 5f; // Dur�e de l'aura
    public float auraRegenRate = 10f; // Taux de r�g�n�ration de l'aura
    public float walkSpeed = 3f; // Vitesse de d�placement
    public bool drawGizmos = true; // Activer/d�sactiver les gizmos
    public float groundYPosition = 0f; // Position Y du sol

    private Animator animator;
    private bool isAuraActive = false;
    private bool hasAuraBeenUsed = false; // Nouvelle variable pour v�rifier si l'aura a �t� utilis�e
    private GameObject currentAuraInstance; // R�f�rence � l'instance actuelle de l'aura
    private Coroutine patteCoroutine; // R�f�rence � la coroutine d'attaque pattes

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (auraPrefab == null)
        {
            Debug.LogError("auraPrefab n'est pas assign� !");
        }

        if (pattePrefab == null)
        {
            Debug.LogError("pattePrefab n'est pas assign� !");
        }

        if (auraSpawnPoint == null)
        {
            Debug.LogError("auraSpawnPoint n'est pas assign� !");
        }

        if (player == null)
        {
            Debug.LogError("player n'est pas assign� !");
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (currentHealth <= healthThreshold && !isAuraActive && !hasAuraBeenUsed)
        {
            TriggerAura();
        }

        // Initialisation du mouvement si n�cessaire
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Walk");
        }
    }

    private void TriggerAura()
    {
        isAuraActive = true;
        hasAuraBeenUsed = true; // Marquer l'aura comme utilis�e
        animator.SetTrigger("Aura");
    }

    public void SpawnAura()
    {
        if (auraPrefab != null && auraSpawnPoint != null)
        {
            currentAuraInstance = Instantiate(auraPrefab, auraSpawnPoint.position, auraSpawnPoint.rotation);
            Debug.Log("Aura instanci�e");
        }
        else
        {
            Debug.LogError("�chec de l'instanciation de l'aura : le prefab ou le point de spawn est null");
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
            yield return new WaitForSeconds(0.5f); // Temps entre chaque instantiation, ajustez selon vos besoins
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

    public void DestroyAura()
    {
        if (currentAuraInstance != null)
        {
            Destroy(currentAuraInstance);
            Debug.Log("Aura d�truite");
        }
        isAuraActive = false;
    }

    // Dessiner les gizmos pour la port�e de d�tection et la port�e de smash
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, smashRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}

