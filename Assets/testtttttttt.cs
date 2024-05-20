using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // R�f�rence au joueur et � l'objet � instancier
    public GameObject player;
    public GameObject objectToSpawn;
    public float groundYPosition;

    void Start()
    {
        // Assurez-vous que les r�f�rences sont d�finies
        if (player == null || objectToSpawn == null)
        {
            Debug.LogError("Player or ObjectToSpawn not set.");
            return;
        }
    }

    void Update()
    {
        // V�rifier si la touche "N" est press�e
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnObjectAtPlayerPosition();
        }
    }

    void SpawnObjectAtPlayerPosition()
    {
        // R�cup�rer la position X du joueur
        float playerXPosition = player.transform.position.x;

        // Cr�er la position pour le nouvel objet
        Vector3 spawnPosition = new Vector3(playerXPosition, groundYPosition, 0);

        // Instancier l'objet � la position d�termin�e
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}