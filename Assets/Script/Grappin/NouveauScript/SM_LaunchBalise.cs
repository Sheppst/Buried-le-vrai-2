using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Balise_SM;

// Ce script envoie une balise qui sera suivie d'un trait corresopandt � un grappin

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
        Prog = new Process(); // Cr�er une nouvelle machine d'�tat
        if (Prog.CurrentState == ProcessState.Inactive)
        {
            transform.SetParent(Player); // L'objet deviens enfant du joueur
            transform.position = Vector3.zero; // Et se ressitue sur le centre du joueur
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0f;
        hit = targetPosition;
        if (Prog.CurrentState == ProcessState.Inactive) // Quand la balise n'est pas mobilis� et qu'elle est sur le joueur
        {
            // //IDEE : Cr�er une condition qui v�rifie si en inactif on est bien sur le joueur et pas ailleur 
            StopAllCoroutines(); // Pr�caution 
            transform.SetParent(Player); // Red�f�nis les m�mes condition que dans le Start
            transform.position = Vector3.zero;
        }
        if (Prog.CurrentState == ProcessState.Throwed) // Quand la balise est envoy� vers la derni�re position de la souris 
        {
            Rigid.WakeUp(); // Fait en sorte que le rigibody de l'objet marche
            transform.SetParent(null); // N'est plus enfant de personne
            if (OnMove) // Si la variable est ON donne une position o� se dirigera 
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
        if (Input.GetMouseButtonDown(0) && Prog.CurrentState == ProcessState.Inactive) // Si j'enl�ve la condition OffContact alors la balise revient je ne sais pas pourquoi aled XD
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
        yield return new WaitForSecondsRealtime(3f); //Permet � la balise d'aller jusqu'� une certaine distance, sinon autorise � ramener la balise
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
