using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DetectBite : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject Bite;
    private Transform current;
    public bool Lock;
    // Update is called once per frame
    private void OnEnable()
    {
        current = GetComponentInParent<Phase01>().Current;
        Bite.SetActive(false);
    }
    void Update()
    {
        RaycastHit2D AttackRange = Physics2D.Raycast(transform.position, transform.right, 2, layer);
        if (AttackRange.collider != null)
        {
            Bite.SetActive(true);
            Debug.DrawLine(transform.position, AttackRange.point, Color.yellow);
            Lock = true;
        }
        else
        {
            Debug.DrawLine(transform.position, new Vector3 (current.position.x,transform.position.y,transform.position.z) , Color.red);
            Lock = false;
        }
    }
}
