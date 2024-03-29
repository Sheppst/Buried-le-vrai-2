using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LaunchBalise : MonoBehaviour
{
    [SerializeField] private LayerMask defaultlayer;
    [SerializeField] private Transform Player;
    [SerializeField] private float speed = 20;
    [SerializeField] private Rigidbody2D Rigid;
    private bool OnMove;
    private bool InMove = true;
    private bool OffContact;
    private bool Keep;
    private bool Debug01;
    private bool Debug02;
    [HideInInspector] public bool Connect;
    [HideInInspector] public bool Spawn;
    private Vector3 hit;
    private void Start()
    {
        transform.SetParent(null);

    }
    // Update is called once per frame
    void Update()
    {
        if (!OnMove && !OffContact && !Keep) 
        {
            transform.position = Player.position;
        }
        if (Input.GetMouseButtonDown(0) && !OffContact) // Si j'enlève la condition OffContact alors la balise revient je ne sais pas pourquoi aled XD
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0f;
            hit = targetPosition;
            OnMove = true;
        }
        if (Input.GetMouseButton(0))
        {
            Keep = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Rigid.WakeUp();
            Keep = false;
            OffContact = true;
            OnMove = false;
        }
        if (OnMove)
        {
            Rigid.WakeUp();
            if (InMove)
            {
                Vector2 Target = (hit - transform.position).normalized;
                Rigid.velocity = 10 * speed * Time.deltaTime * Target;
                InMove = false;
                Debug01 = true;
                StartCoroutine(Direction());
            }
        }
        else if (OffContact || !Keep)
        {
            StopCoroutine(Direction());
            StartCoroutine(ReturnPlayer());
        }

    }
    private IEnumerator Direction()
    {
        yield return new WaitForSecondsRealtime(3f); //Permet à la balise d'aller jusqu'à une certaine distance, sinon autorise à ramener la balise
        if (OnMove) 
        {
            OffContact = true;
            OnMove = false;
        }
        Debug01 = false;
    }
    private IEnumerator ReturnPlayer()
    {
        Vector2 Target = (Player.position - transform.position).normalized;
        Rigid.velocity = 10 * speed * Time.deltaTime * Target;
        yield return new WaitForSecondsRealtime(0.01f); //Permet de redonner la direction à la cible 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            OffContact = false;
            InMove = true;

            Spawn = true;
            Rigid.Sleep();
            Debug02 = false;
        }
        else
        {
            Rigid.Sleep();
            StopCoroutine(Direction());
            OnMove = false;
            Connect = true;
            if (!Keep)
            {
                OffContact = true;
            }
            Debug02 = true;
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Keep)
        {
            OffContact = false;
            InMove = true;
            Rigid.Sleep();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Spawn = false;
        }
        else
        {
            Connect = false;
        }
    }
}
