using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMovement : MonoBehaviour
{
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;
    [SerializeField] private Transform Center;
    public float Life = 100;
    private List<GameObject> er;
    // Start is called before the first frame update
    void Start()
    {
        Left.parent = null; 
        Right.parent = null;
        Center.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
