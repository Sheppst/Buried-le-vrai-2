using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlayer : MonoBehaviour
{
    public Ascenceur Ascenceur;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.O)) 
        {
            Ascenceur.Canelevate = true;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
     private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Stop")
        {
           Ascenceur.Canelevate = false;
        }
    }
}
