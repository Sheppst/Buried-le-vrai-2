using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Detect : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [HideInInspector] public bool Player;
    [HideInInspector] public Transform MobMvt;
    public Transform right;
    public Transform left;
    public Transform current;
    private float Turn = 180f;
    // Start is called before the first frame update
    void Start()
    {
        MobMvt = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Turn);
        current = GetComponentInParent<MobMovement>().Current;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, 10f, layer);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, transform.right * hit.distance, Color.yellow);
            if (layer == 8)
            {
                Player = true;
            }
            if (current == right)
            {
                Turn = 0;
            }
            else if (current == left) 
            {
                Turn = 180;
            }


        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * 10, Color.red);
            if (layer == 8)
            {
                Player = false;
            }
            if (current == right)
            {
                if (MobMvt.position.x < right.position.x) 
                { 
                    Turn = 0f;
                }
                else
                {
                    Turn = 180f;
                }
            }
            else if (current == left)
            {
                if (MobMvt.position.x > left.position.x)
                {
                    Turn = 180;
                }
                else
                { 
                    Turn = 0; 
                }
            }
        }
        
    }
}
