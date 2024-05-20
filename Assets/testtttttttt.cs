using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Référence au joueur et à l'objet à instancier
    public GameObject player;
    public GameObject objectToSpawn;
    public float groundYPosition;

    void Start()
    {
        // Assurez-vous que les références sont définies
        if (player == null || objectToSpawn == null)
        {
            Debug.LogError("Player or ObjectToSpawn not set.");
            return;
        }
    }

    void Update()
    {
        // Vérifier si la touche "N" est pressée
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnObjectAtPlayerPosition();
        }
    }

    void SpawnObjectAtPlayerPosition()
    {
        // Récupérer la position X du joueur
        float playerXPosition = player.transform.position.x;

        // Créer la position pour le nouvel objet
        Vector3 spawnPosition = new Vector3(playerXPosition, groundYPosition, 0);

        // Instancier l'objet à la position déterminée
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}