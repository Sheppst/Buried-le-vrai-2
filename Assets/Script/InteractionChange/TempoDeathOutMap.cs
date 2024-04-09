using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ceci est un script qui est utilisé pour un gameobject
// donnant une certaine logique de mort du joueur s'il arrive à sortir de la map

public class TempoDeathOutMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(0); // Change de scène pour aller au menu
        }
    }
}
