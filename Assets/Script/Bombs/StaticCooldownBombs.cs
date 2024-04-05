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
    // Start is called before the first frame update
    void Awake()
    {
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(StaticExplosion());
    }
    private IEnumerator StaticExplosion()
    {
        yield return new WaitForSeconds(1)
        {
        };
        rigid.bodyType = RigidbodyType2D.Kinematic;
        collid.isTrigger = true;
        collid.radius = 8;
        gameObject.tag = "ExploStatTrue";
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        
    }
}
