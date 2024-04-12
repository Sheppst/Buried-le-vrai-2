using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInActive : MonoBehaviour
{
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        transform.position = new Vector3(-16, 1, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
