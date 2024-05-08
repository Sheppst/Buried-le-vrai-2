using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWallV : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    [SerializeField] float Speed;
    public bool Descend;
    private bool Nothing;
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
        if (Descend && transform.parent.GetComponent<SM_MobVolantDistance>().CurrStat("DetectStmh") && transform.parent.GetComponent<SM_MobVolantDistance>().CurrStat("AttackStmh"))
        {
            transform.parent.position += Vector3.up * Speed * Time.deltaTime;
        }

    }
}
