using UnityEngine;

public class InstantiateAtGroundLevel : MonoBehaviour
{
    public GameObject prefab; // Le prefab à instancier
    public Transform playerTransform; // Référence au transform du joueur
    public string groundTag = "Ground"; // Tag pour identifier le sol

    void Update()
    {
        // Condition pour instancier (par exemple, lorsqu'on appuie sur la touche espace)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InstantiatePrefabAtGroundLevel();
        }
    }

    void InstantiatePrefabAtGroundLevel()
    {
        if (prefab != null && playerTransform != null)
        {
            // Effectuer un raycast vers le bas depuis la position du joueur pour détecter le sol
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, Mathf.Infinity);

            // Visualiser le raycast dans la vue de la scène
            Debug.DrawRay(playerTransform.position, Vector2.down * 100, Color.red, 2f);

            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                Debug.Log("Sol détecté à la position : " + hit.point);

                // Calculer la position au niveau du sol mais alignée horizontalement avec le joueur
                Vector3 positionAtGroundLevel = new Vector3(playerTransform.position.x, hit.point.y, playerTransform.position.z);

                // Instancier le prefab à la position calculée sans rotation
                Instantiate(prefab, positionAtGroundLevel, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Sol non détecté ou tag incorrect.");
            }
        }
        else
        {
            Debug.LogError("Prefab ou PlayerTransform non défini.");
        }
    }
}