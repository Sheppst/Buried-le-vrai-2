using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ce script est utilisé dans le cadre du MobTerre, quel qu'il soit

public class SM_Detect : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private bool Player;
    [HideInInspector] public Transform MobMvt;
    public Transform right;
    public Transform left;
    public Transform current;
    // Start is called before the first frame update
    void Start()
    {
        MobMvt = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 10f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.right * hit.distance, Color.yellow);
                Player = true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.right * hit.distance, Color.red);
                Player = false;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * 10, Color.red);
            Player = false;
        }
    }
    public bool Hit()
    {
        return Player;
    }
}
