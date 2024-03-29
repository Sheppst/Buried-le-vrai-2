using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactGround : MonoBehaviour
{
    [HideInInspector] public bool ascend;
    private bool locked;
    private Rigidbody2D MyRigid;

    // Start is called before the first frame update
    void Start()
    {
        MyRigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ascend && !locked)
        {
            MyRigid.gravityScale = -MyRigid.gravityScale;
            locked = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            ascend = true;
        }
    }
}
