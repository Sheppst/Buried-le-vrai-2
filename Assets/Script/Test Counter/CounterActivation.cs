using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterActivation : MonoBehaviour
{
    public Collider2D CounterReact;
    public Transform ChildPosition;
    private bool cooldown = true;
    private bool right;
    private bool left;
    private float speed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        CounterReact.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown) 
        {
            if (Input.GetKeyUp(KeyCode.Space)) 
            {
                print("Activate");
                CounterReact.enabled = true;
                StartCoroutine(Desactivation());
            }
        }
        if (right) 
        {
            transform.position += Vector3.right * speed;
            if (ChildPosition.localScale.x == -1)
            {
                ChildPosition.localScale = new Vector3(-ChildPosition.localScale.x, ChildPosition.localScale.y, ChildPosition.localScale.z);
            }
        }
        if (left)
        {
            transform.position += Vector3.left * speed;
            if (ChildPosition.localScale.x == 1)
            {
                ChildPosition.localScale = new Vector3(-ChildPosition.localScale.x, ChildPosition.localScale.y, ChildPosition.localScale.z);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            right = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            right = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            left = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            left= false;
        }
    }
    private IEnumerator Desactivation()
    {
        yield return new WaitForSeconds(0.05f);
        CounterReact.enabled = false;
    }
    private IEnumerator Cooldown() 
    {
        cooldown = false;
        yield return new WaitForSeconds(1f);
        cooldown = true;
    }
}
