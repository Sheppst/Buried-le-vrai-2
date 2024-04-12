using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInActive : MonoBehaviour
{
    private bool OnLerp;
    public float speed = 3;
    private float Zslasher = 1;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        transform.position = new Vector3(-16, 1, 0);
        OnLerp = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            OnLerp = true;
        }
        if (OnLerp)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.Lerp(transform.position, target - Vector3.forward * Zslasher, speed * Time.deltaTime);
            OnLerp = false;
            Zslasher++;
        }
    }
}
