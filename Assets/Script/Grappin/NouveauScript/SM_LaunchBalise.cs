using Balise_SM;
using System.Collections;
using UnityEngine;

// Ce script envoie une balise qui sera suivie d'un trait corresopandt à un grappin

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
        Prog = new Process(); // Créer une nouvelle machine d'état
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
        if (Prog.CurrentState == ProcessState.Inactive) // Quand la balise n'est pas mobilisé et qu'elle est sur le joueur
        {
            // //IDEE : Créer une condition qui vérifie si en inactif on est bien sur le joueur et pas ailleur 
            StopAllCoroutines(); // Précaution 
            transform.SetParent(Player); // Redéfénis les mêmes condition que dans le Start
            transform.position = Vector3.zero;
        }
        if (Prog.CurrentState == ProcessState.Throwed) // Quand la balise est envoyé vers la dernière position de la souris 
        {
            Rigid.WakeUp(); // Fait en sorte que le rigibody de l'objet marche
            transform.SetParent(null); // N'est plus enfant de personne
            if (OnMove) // Si la variable est ON donne une position où se dirigera la balise une seule fois 
            {
                Vector2 Target = (hit - transform.position).normalized; // Calcule un vecteur entre la position original et la position voulu
                Rigid.velocity = 10 * speed * Target; // Applique le vecteur à la vélocité de l'objet, lui ajoutant une vitesse contenant un modificateur de x10
                OnMove = false; // Empêche l'actualistation de la direction
                StartCoroutine(MaxDist()); // Démarre une coroutine qui ramenéra la balise si aucun contact n'est fait 
            }
            if (Input.GetMouseButtonUp(0)) // Si [Souris Gauche] est relaché, alors ... // Enclenche un effet de retour vers le joueur
            {
                StopAllCoroutines(); // Arrête MaxDist et tout autre coroutines 
                OnMove = true; // Rend le changement de direction à nouveau possible au prochain passage de phase
                Prog.MoveNext(Command.End); // Transition entre Throwed -> NoContacted
            }
        }
        if (Prog.CurrentState == ProcessState.Contacted) // Si la balise rentre en contact avec un objet accrochable 
        {
            StopAllCoroutines(); // Précaution
            Rigid.Sleep(); // Bloque la balise à sa position en désactivant la physique de l'objet
            if(Input.GetMouseButtonUp(0)) // Si [Souris Gauche] est relaché alors... // Enclenche un effet de retour vers le joueur
            {
                Prog.MoveNext(Command.End); // Transition de Contacted -> NoContacted
            }
        }
        if (Prog.CurrentState == ProcessState.NoContacted) // Si la balise revient vers le joueur et ne doit rien toucher 
        {
            StopAllCoroutines(); // Arrête par précaution toutes les coroutines 
            Rigid.velocity = Vector3.zero; // Bloque la vélocité de l'objet à 0 pour éviter tous ancien mouvement plausiblement conservé 
            Rigid.Sleep(); // Le grappin va de toute façon revenir sur le joueur ainsi on ne va pas utiliser la physique pour déplacer l'objet cette fois-ci 
                           // Désactive la physique de l'objet
            transform.position = Vector3.MoveTowards(transform.position,Player.position, speed * Time.deltaTime /100); 
            // Va faire bouger manuellement l'objet vers le joueur, sous une certaine vitesse que l'ont mettra sur une temps précalculer avec un modificateur de /100
        }
        if (Input.GetMouseButtonDown(0) && Prog.CurrentState == ProcessState.Inactive) // Si la balise est désactivé et que [Souris Gauche] est presser 
                                                                                       // Déclenche un effet de projection du grappin vers le dernier endroit selectioner par la position de la souris 
        {
            Prog.MoveNext(Command.Begin); // Transition Inactive -> Throwed
        }

    }
    public bool ReturnState( string State) // Fonction publique permettant de connaître l'état actif
                                           // Seul Contact et Inactive sont vérifié sont présent de ce côté car je n'en ai pas besoin de l'autre côté 
    {// On entre en string la phase que l'on veut vérifié, puis on range cet état dans une variable et vérifie si cette variable correspond avec la phase actuel
        if(State == "Contact")
        {
            CS = ProcessState.Contacted;
        }
        if (State == "Inactive")
        {
            CS = ProcessState.Inactive;
        }
        else
        {
            return false;
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
        yield return new WaitForSeconds(3f * Time.deltaTime); //Permet à la balise d'aller jusqu'à une certaine distance, sinon autorise à ramener la balise
        if (Prog.CurrentState == ProcessState.Throwed)
        {
            Prog.MoveNext(Command.End); // Effectue transition Throwed -> NoContacted
            OnMove = true; // Réactive la variable pour le prochain ppassage de phase
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Prog.CurrentState == ProcessState.NoContacted) // Si la balise revient au contact du joueur le joueur
        {
            //Spawn = true;
            Prog.MoveNext(Command.End); // Transition NoContacted -> Inactive
        }
        else if (Prog.CurrentState == ProcessState.Throwed) // Si la balise touche un objet d'accroche
        {
            //Connect = true;
            Prog.MoveNext(Command.Pause); // Transition Throwed -> Contacted
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Prog.CurrentState != ProcessState.Inactive)
        {
            Prog.MoveNext(Command.End); // 
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
