using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastRooffed : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    private bool HT; 
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D Nohit = Physics2D.Raycast(transform.position, transform.up, 3f, layer);

        if (Nohit.point == null && Nohit.collider.gameObject.tag != "Player")
        {
            Vector3 EndRaycast = transform.position + Vector3.up * 3;
            Debug.DrawLine(transform.position, EndRaycast, Color.blue);
        }
        else
        {
            Debug.DrawLine(transform.position, Nohit.point, Color.red);
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 0.5f, layer);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].gameObject != gameObject) // La détection va forcément détecter l'objet lui-même s'il possède un collider donc on cherche à éviter ça.
                                                     // Et si un autre objet entre en collision déclenche la condition.
                {
                    Debug.DrawLine(transform.position, hit[i].gameObject.transform.position, Color.red);
                    break;
                }
                else
                {
                    HT = true;
                }
            }
            if (HT)
            {
                Vector3 EndRaycast = transform.position + Vector3.up * 0.5f;
                Debug.DrawLine(transform.position, EndRaycast, Color.yellow);
                transform.parent.position += Vector3.up * 0.01f;
            }
        }
    }
}
