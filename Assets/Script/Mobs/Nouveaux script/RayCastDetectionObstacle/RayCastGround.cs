using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGround : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    [SerializeField] float Speed;
    private bool Nothing;
    public bool Descend;
    // Update is called once per frame
    void Update()
    {
        Collider2D[] Ground = Physics2D.OverlapCircleAll(transform.position, .2f);
        Nothing = true;
        for (int i = 0; i < Ground.Length; i++)
        {
            if (Ground[i] != transform.parent.gameObject)
            {
                Nothing = false;
            }
        }
        if (Nothing)
        {
            Descend = true;
        }
        if (Descend)
        {
            transform.parent.position += Vector3.down * Speed * Time.deltaTime;
        }

    }
}
