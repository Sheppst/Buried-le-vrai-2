using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interface;

public class MouseDamage : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast
                    (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layer);
            Interactable coll = hit.collider.gameObject.GetComponent<Interactable>();
            if (hit.collider != null)
            {
                coll.InteractOwn();   
            }
        }
    }
}
