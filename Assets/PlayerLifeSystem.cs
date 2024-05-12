using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeSystem : MonoBehaviour
{
    
    public float lifePool = 100;
    [HideInInspector] public const float maxLife = 100f;
    

    void Start()
    {
        
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mob")
        {
            lifePool -= 5;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mob" || collision.gameObject.tag == "pics")
        {
            lifePool -= 5;
        }

    }
}
