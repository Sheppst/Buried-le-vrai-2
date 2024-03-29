using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Balise_SM;

public class SM_LaunchBalise : MonoBehaviour
{
    [SerializeField] private LayerMask defaultlayer;
    [SerializeField] private Transform Player;
    [SerializeField] private float speed = 500;
    [SerializeField] private Rigidbody2D Rigid;
    private bool OnMove;
    [HideInInspector] public bool Connect;
    [HideInInspector] public bool Spawn;
    private Vector3 hit;
    Process Prog;
    ProcessState CS;
    private void Start()
    {
        Prog = new Process();
        if (Prog.CurrentState == ProcessState.Inactive)
        {
            transform.SetParent(Player);
            transform.position = Vector3.zero;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0f;
        hit = targetPosition;
        if (Prog.CurrentState == ProcessState.Inactive)
        {
            StopAllCoroutines();
            transform.SetParent(Player);
            transform.position = Vector3.zero;
        }
        if (Prog.CurrentState == ProcessState.Throwed)
        {
            Rigid.WakeUp();
            transform.SetParent(null);
            if (OnMove)
            {
                Vector2 Target = (hit - transform.position).normalized;
                Rigid.velocity = 10 * speed * Target;
                OnMove = false;
                StartCoroutine(MaxDist());
            }
            if (Input.GetMouseButtonUp(0))
            {
                StopAllCoroutines();
                OnMove = true;
                Prog.MoveNext(Command.End);
            }
        }
        if (Prog.CurrentState == ProcessState.Contacted) 
        {
            StopAllCoroutines();
            Rigid.Sleep();
            if(Input.GetMouseButtonUp(0))
            {
                Prog.MoveNext(Command.End);
            }
        }
        if (Prog.CurrentState == ProcessState.NoContacted)
        {
            Rigid.velocity = Vector3.zero;
            Rigid.Sleep();
            StopAllCoroutines();
            transform.position = Vector3.MoveTowards(transform.position,Player.position, speed * Time.deltaTime /100);
        }
        if (Input.GetMouseButtonDown(0) && Prog.CurrentState == ProcessState.Inactive) // Si j'enlève la condition OffContact alors la balise revient je ne sais pas pourquoi aled XD
        {
            Prog.MoveNext(Command.Begin);
        }

    }
    public bool ReturnState( string State)
    {
        if(State == "Contact")
        {
            CS = ProcessState.Contacted;
        }
        if (State == "Inactive")
        {
            CS = ProcessState.Inactive;
        }
        if ( CS == Prog.CurrentState)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    private IEnumerator MaxDist()
    {
        yield return new WaitForSecondsRealtime(3f); //Permet à la balise d'aller jusqu'à une certaine distance, sinon autorise à ramener la balise
        Prog.MoveNext(Command.End);
        OnMove = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Spawn = true;
            Prog.MoveNext(Command.End);
        }
        else
        {
            //Connect = true;
            Prog.MoveNext(Command.Pause);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Prog.CurrentState != ProcessState.Inactive)
        {
            Prog.MoveNext(Command.End);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Spawn = false;
        }
        else if (Prog.CurrentState == ProcessState.Contacted)
        {
            Prog.MoveNext(Command.End);
            //Connect = false;
        }
    }
}
