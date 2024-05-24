using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticCooldownBombs : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private CircleCollider2D collid;
    [SerializeField] private float throwingspeed;
    public GameObject Player;
    private AudioSource audioSource;
    private bool Tabarnac;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!Tabarnac)
        {
            StartCoroutine(StaticExplosion());
            Tabarnac = true; // Tabarnac de chti!!!!!!!!!
        }
    }
    private IEnumerator StaticExplosion()
    {
        yield return new WaitForSeconds(1);
        audioSource.Play();
        rigid.bodyType = RigidbodyType2D.Kinematic;
        collid.isTrigger = true;
        collid.radius = 8;
        gameObject.tag = "ExploStatTrue";
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        
    }
}
