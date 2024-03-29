using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamModifier : MonoBehaviour
{
    private Rigidbody2D PlayerRigid;
    private float Ontheright;
    private float Ontheleft;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigid = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerRigid.velocity.y <= 2)
        {
            transform.position = PlayerRigid.velocity;
        }

    }
}
