using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrucBloc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ExploStatTrue" || collision.gameObject.tag == "ExploThrTrue")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ExploStatTrue" || collision.gameObject.tag == "ExploThrTrue")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
