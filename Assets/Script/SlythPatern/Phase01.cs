using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SM_Phase01;

public class Phase01 : MonoBehaviour
{
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;
    [SerializeField] private string NomDuJoueur;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject Ray;
    public Vector3 Newpos;
    private Rigidbody2D rigid;
    private float Life;
    private float speed = 5f;
    private bool Flip;
    private Transform Player;
    private PolygonCollider2D colid;
    public Transform Current;
    Process Prog;
    ProcessState CS;
    // Start is called before the first frame update
    void Start()
    {
        Life = transform.parent.gameObject.GetComponent<AllMovement>().Life;
        colid = GetComponent<PolygonCollider2D>();
        rigid = GetComponentInParent<Rigidbody2D>();
        Prog = new Process();
        Player = GameObject.Find(NomDuJoueur).GetComponent<Transform>();
        if ( Prog.CurrentState == ProcessState.Inactive)
        {
            Prog.MoveNext(Command.Begin);
        }
        if (transform.position.x > Player.position.x)
        {
            Current = Left;
            Ray.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);

        }
        else if (transform.position.x < Player.position.x)
        {
            Current = Right;
            Ray.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }

    }

    // Update is called once per frame
    void Update()
    {
        ProcessState CSN = Prog.CurrentState;
        if (CS != CSN)
        {
            CS = CSN;
            print(CS);
        }
        if (Life <= 0)
        {
            Prog.MoveNext(Command.Death);
        }
        Player = GameObject.Find(NomDuJoueur).GetComponent<Transform>();
        


        if (Prog.CurrentState == ProcessState.Terminated) 
        {
            StopAllCoroutines();
        }
        if (Prog.CurrentState == ProcessState.Inactive) //Etat de reset du boss 
        {
            if (transform.position.x > Player.position.x) // Si le joueur se trouve à gauche ...
            {
                Current = Left; // ... L'objectif de déplacement devient la quache
                Ray.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180); //... Le raycast se tourne vers la gauche 
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z); //... Le boss se retourne vers le joueur 
                
            }
            else if (transform.position.x < Player.position.x) // Vice-versa
            {
                Current = Right;
                Ray.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                
            }
            StartCoroutine(Wait()); // Lance un petit cooldown, puis change d'état
        }
        if (Prog.CurrentState == ProcessState.ChoisingPhase) // Le boss choisit son patern...
        {
            StopAllCoroutines(); // précaution
            float distC = Vector3.Distance(Current.position, transform.position); // Distance entre la position du boss et de sa destination
            float distP = Vector3.Distance(Player.position, transform.position); // Distance entre la position du boss et le joueur 
            if (distC/2 > distP) // ... si le joueur est proche,
                                 // car la distance entre la position entre le boss et son objectif reste supérieur à la distance
                                 // avec le joueur malgré le fait qu'elle divisé par deux 
            {
                Prog.MoveNext(Command.Bit); // Changement d'état de ChoisingPhase -> Bited
                Newpos =  new Vector3(Player.position.x, transform.position.y, transform.position.z); // Dernier endroit que le boss à vu le joueur
                Ray.SetActive(true); // L'objet tenant la détection s'active                                                            
            }
            else if (distC/2 < distP) // ...si le joueur est trop loin
            {
                Prog.MoveNext(Command.CutPhase); // Changement spécial pour rendre dans les temps, ChoisingPhase -> Charge
            }
        }
        if (Prog.CurrentState == ProcessState.Bited) // Quand le boss cherche à mordre le joueur 
        {
             
            if (Ray.activeSelf == false) // Si l'objet de détection n'est pas actif...
            {
                Prog.MoveNext(Command.CutPhase); // ... Changement d'état de Bited -> Inactive 
            }
            else if (!GetComponentInChildren<DetectBite>().Detect )
            {
                transform.position = Vector3.MoveTowards(transform.position, Newpos, speed * Time.deltaTime); // Déplace le boss vers la dernière position du joueur connu
            }

        }
        if (Prog.CurrentState == ProcessState.Charge) // Quand le boss cherche à chager le joueur 
        {
            Vector3 cur = new Vector3(Current.position.x, transform.position.y, transform.position.z); // prend uniquement la position de l'objectif en X 
            Vector2 target = (cur - transform.position).normalized; // Calcule un vecteur de direction entre la position du boss et de l'objectif en restant sur l'axe X
            rigid.velocity = target * speed; // Lance le vecteur à une certaine vitesse sur la vélocité
        }
        if (Prog.CurrentState == ProcessState.OnRooffed)
        {
            StopAllCoroutines();
        }
        if (Prog.CurrentState == ProcessState.ThrowedPoison)
        {
            StopAllCoroutines();
        }
        if (Prog.CurrentState == ProcessState.ThrowedWeb)
        {

        }
        if (Prog.CurrentState == ProcessState.SuccessBite)
        {

        }
        if (Prog.CurrentState == ProcessState.SuccessCharged)
        {

        }
        if (Prog.CurrentState == ProcessState.SuccessPoisoned)
        {
            StopAllCoroutines();
        }
        if (Prog.CurrentState == ProcessState.SuccessStunned)
        {

        }
        
        
        if(1==0)
        {

        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        if (Prog.CurrentState == ProcessState.Inactive) // Temps d'attente pour le cooldown en Inactive
        {
            Prog.MoveNext(Command.Begin); // Changement d'état entre Inactive -> ChoicePhase
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Damageable") // Prend effet si le boss rentre au collision avec un objet pouvant l'endomager 
        {
            print("touché"); // Pour les tests futures
        }
        if (Prog.CurrentState == ProcessState.Charge) // Si durant la phase de Charge ...
        {
            if (collision != null ) // /... Le boss rencotre n'importe quel obstacle, alors ...
            {
                rigid.velocity = Vector2.zero; // ... Le boss arrête de bouger
                Prog.MoveNext(Command.CutPhase); // ... Effectue une transition d'état de Charge -> Inactive
            }
        }
    }
}
