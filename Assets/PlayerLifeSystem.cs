using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeSystem : MonoBehaviour
{
    
   [SerializeField] public float Life = 100;

    void Start()
    {
        
    }

   
    void Update()
    {
        if (Life <= 0f)
        {
            SceneManager.LoadScene(0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mob")
        {
            Life -= 5;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mob" || collision.gameObject.tag == "pics")
        {
            Life -= 5;
        }

    }
}
