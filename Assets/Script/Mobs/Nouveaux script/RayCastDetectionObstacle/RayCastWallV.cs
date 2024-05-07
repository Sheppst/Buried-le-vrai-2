using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWallV : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    [SerializeField] GameObject Detects;
    public bool Change;
    private bool Nohit;
    // Update is called once per frame
    void Update()
    {
        if (transform.parent.localScale.x < 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (transform.parent.localScale.x > 0)
        {
            transform.eulerAngles = Vector3.right * 180;
        }
        //if (!Change)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, layer);
        //    if (hit.collider.gameObject.tag != "Player" && hit.point != null)
        //    {
        //        Detects.GetComponent<RayCastRooffed>().Change = true;
        //        Debug.DrawLine(transform.position, hit.point, Color.yellow);
        //        transform.parent.position += Vector3.down * 0.01f;
        //    }
        //    else
        //    {
        //        Detects.GetComponent<RayCastRooffed>().Change = false;
        //        Vector3 EndRaycast = transform.position + Vector3.left;
        //        Debug.DrawLine(transform.position, EndRaycast, Color.red);
        //    }
        //}
        Nohit = true;
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, .1f,layer);
        bool ascend = false;
        for (int i = 0; i < hit.Length; i++)
        {

            if ( hit.Length <= 1 && !ascend)
            {
                break;
            }
            if (hit[i].tag != "Player")
            {
                ascend = true;
                transform.parent.position += Vector3.down * 0.01f;
            }
            Nohit = false;
        }
    }
}
