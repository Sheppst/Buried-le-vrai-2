using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayCastRooffed : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    [SerializeField] GameObject[] Detects;
    private bool HT;
    public bool Change;
    // Update is called once per frame
    void Update()
    {
        if (!Change)
        {
            HT = false;
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
                    if (hit[i].gameObject != gameObject) // La d�tection va forc�ment d�tecter l'objet lui-m�me s'il poss�de un collider donc on cherche � �viter �a.
                                                         // Et si un autre objet entre en collision d�clenche la condition.
                    {
                        Debug.DrawLine(transform.position, hit[i].gameObject.transform.position, Color.red);
                        HT = true;
                        break;
                    }
                }
                if (!HT)
                {
                    transform.parent.position += Vector3.up * 0.01f;
                }
            }
        }
    }
}
