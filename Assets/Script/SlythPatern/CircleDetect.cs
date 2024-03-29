using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Collider2D Bited;

    private void OnEnable()
    {
        StopAllCoroutines();
        Bited.enabled = false;
        StartCoroutine(ReactionTime());
    }
    private IEnumerator ReactionTime() 
    {
        yield return new WaitForSeconds(0.5f);
        bool Lock = GameObject.Find("DetectRay").GetComponent<DetectBite>().Lock;
        if (Lock)
        {
            Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.5f, layer);
            if (IsBited(collision))
            {
                Bited.enabled = true;
            }
        }
        gameObject.SetActive(false);
    }
    private bool IsBited(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            return true;
        }
        return false;
    }
}
