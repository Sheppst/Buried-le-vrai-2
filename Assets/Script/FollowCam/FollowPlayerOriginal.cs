using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerOriginal : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D rigid;
    private float speed = 50f; 
    private bool stat = true;
    [SerializeField] private float UpCam;


    void Start()
    {
        target = GameObject.Find("CamPosition").transform;
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
            
        if (target == null) 
        {
            target = transform; 
        }
        if (target != null && stat) 
        {
            StartCoroutine(Focus()); 
            stat = false; 
        }
    }
    IEnumerator Focus()
    {
        Vector2 direction = ((target.position + Vector3.up * UpCam) - transform.position).normalized;
        rigid.velocity = direction * speed; 
        yield return new WaitForSeconds(0.01f);
        speed = 50f;
        stat = true; 

    }
}
