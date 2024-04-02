using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    public void After ()
    {
        Destroy(gameObject);
    }
}
