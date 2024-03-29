using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowCooldownBombs : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private CircleCollider2D collid;
    [SerializeField] private float throwingspeed;
    private bool Contact;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 direction = Player.transform.right * throwingspeed;
        direction.y = 300; 
        rigid.AddForce(direction); 
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject == null) 
        {
            StopAllCoroutines();
        }
        if (Contact) 
        {
            StartCoroutine(CetteFonctionNeDevraisPasExister());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Contact = true;
            rigid.bodyType = RigidbodyType2D.Static;
            collid.isTrigger = true;
            collid.radius = 4;
            gameObject.tag = "ExploThrTrue";
            //rigid.bodyType = RigidbodyType2D.Kinematic;
            //rigid.useFullKinematicContacts = true;
            StartCoroutine(Throwexplosion());
        }
    }
    private IEnumerator Throwexplosion()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    private IEnumerator CetteFonctionNeDevraisPasExister()
    {
        yield return new WaitForSeconds(0.05f);
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }
}
