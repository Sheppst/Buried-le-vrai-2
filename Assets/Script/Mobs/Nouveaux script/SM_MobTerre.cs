using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobTerre_SM;

public class SM_MobTerre : MonoBehaviour
{
    [SerializeField] private Transform Mob;
    [SerializeField] private Transform Right;
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Current;
    [SerializeField] private Transform Detect;
    [SerializeField] private GameObject Point;
    [SerializeField] private GameObject Ray;
    [SerializeField] float PatrolDistance;
    //static List<Transform> list;
    private float Life = 10;
    private Transform Player;
    private Transform Direction;
    public float speed;
    private bool Change = true;
    private Vector3 VecteurRotatGauche = new Vector3(0,0,180);
    Process Prog;
    ProcessState CS;
    // Start is called before the first frame update
    void Start()
    {
        
        //Transform[] add = { Right,Left};
        //list.AddRange(add);
        Right.parent = null; 
        Left.parent = null;
        Prog = new Process();
        CS = Prog.CurrentState; // Pour le débug
        if (Prog.CurrentState == ProcessState.Inactive) // Set up les paramêtres de bases du mob
        {
            Right.position = new Vector3(2 * PatrolDistance + transform.position.x, transform.position.y, transform.position.z); // Place la balise de patrouille en fonction de la position du mob et d'une certaine distance
            Left.position = new Vector3(-2 * PatrolDistance + transform.position.x, transform.position.y, transform.position.z); // Place la balise de patrouille en fonction de la position du mob et d'une certaine distance
            Point.transform.position = transform.position;
            Point.transform.parent = Mob; // Acroche le point de ralliement au Mob 
            Current = Left; // Le current de départ est la gauche
            Direction = Current;
            
            Prog.MoveNext(Command.Begin); // Procède au changement d'état de Inactive à Moved
        }
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.Find("Player").GetComponent<Transform>(); // A update en fonction du nom de joueur
        ProcessState CSN = Prog.CurrentState;   //Pour le débug
        if (CS != CSN)
        {
            print(CSN.ToString());
            CS = CSN;
        } // Pour le debug

        if (Life <= 0) 
        {
            Prog.MoveNext(Command.Death);
        }
        if (Prog.CurrentState != ProcessState.DetectSmth && Prog.CurrentState != ProcessState.Damaged) // Les changement ici bas ne doivent pas ce faire si le monstre est en train de détectez quelque chose, de prendre des dégâts.
        {
            StopAllCoroutines(); // Précaution pour éviter instabilité 
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Current.position.x,transform.position.y,transform.position.z), Time.deltaTime * speed); // Mouvement du monstre vers une cible 
            if (transform.position.x > Current.position.x) // Si le monstre est à droite de la cible le raycast pointera sur la gauche, sur la cible 
            {
                Ray.transform.eulerAngles = VecteurRotatGauche;
            }
            else if (transform.position.x < Current.position.x) // Et inversement
            {
                Ray.transform.eulerAngles = Vector3.zero;
            }
        }
        if (Prog.CurrentState == ProcessState.Terminated) // Si le Mob meurt
        {
            StopAllCoroutines(); // Précaution pour éviter instabilité
            Destroy(gameObject);
        }
        if (Prog.CurrentState == ProcessState.Inactive) // S'il reprend sa routine 
        {
            StopAllCoroutines(); // Précaution pour éviter instabilité
            Right.position = new Vector3(2*PatrolDistance + transform.position.x, transform.position.y, transform.position.z); // Re-initialise les point de patrouille en fonction de la nouvelle position du mob
            Left.position = new Vector3(-2*PatrolDistance + transform.position.x, transform.position.y, transform.position.z); // Re-initialise les point de patrouille en fonction de la nouvelle position du mob
            Point.transform.position = transform.position; // Idem
            Point.transform.parent = Mob; // Idem
            Change = true;
            Prog.MoveNext(Command.Begin); // Idem
        }
        if (Prog.CurrentState == ProcessState.Moved) // S'il se déplace durant sa patrouille
        {
            StopAllCoroutines(); // Précaution pour éviter instabilité
            
            if (transform.position == Current.position) // S'il arrive à destination change son Current
            {
                if (Current == Right)
                {
                    Current = Left;
                    Direction = Left;
                }
                else if (Current == Left)
                {
                    Current = Right;
                    Direction = Right;
                }
            }
            if (Ray.GetComponent<SM_Detect>().Hit()) // Si le Raycast détecte qlq chose 
            {
                Prog.MoveNext(Command.Detect); // Va de Moved à DetecSmth
            }
        }
        if (Prog.CurrentState == ProcessState.DetectSmth) // S'il a l'impression d'avoir détecter quelque chose 
        {
            StartCoroutine(Wait()); // Attend confirmation de la détection
            if (!Ray.GetComponent<SM_Detect>().Hit()) //Sinon ...
            {
                StopAllCoroutines(); // Précaution contre instabilité
                Prog.MoveNext(Command.End); // Passe de DetectSmth à Moved
            }
        }
        if (Prog.CurrentState == ProcessState.AttackSmth) // S'il attaque quelque chose
        {
            StopAllCoroutines(); // Précaution pour éviter instabilité
            Current = Player; // Le joueur devient sa cible 
            if (!Ray.GetComponent<SM_Detect>().Hit()) // Si il n'y a plus de déctection de cible lache l'affaire 
            {
                // Idée : Rajouter le Wait() pour créer une perte d'aggro plus réel
                Prog.MoveNext(Command.End); // Passe de AttackSmth à NoDetect
            }
        }
        if (Prog.CurrentState == ProcessState.NoDetect || Prog.CurrentState == ProcessState.SuccessHit) // S'il cherche à revenir à sa routine
        {     
            Current = Point.transform; // Son current devient le point de ralliement 
            if (Prog.CurrentState == ProcessState.SuccessHit && Change)
            {
                StartCoroutine(Wait());
                Change = false;
            }
            if (Prog.CurrentState == ProcessState.NoDetect && Ray.GetComponent<SM_Detect>().Hit()) // S'il il ne détecte plus sa cible
            {
                Prog.MoveNext(Command.Detect); // Passe de l'état NoDectect à AttackSmth
            }
            if (transform.position == Current.position)
            {
                StopAllCoroutines(); // Précaution pour éviter instabilité
                if (Direction == Right)
                {
                    Current = Left;
                    Direction = Left;
                }
                else if (Direction == Left)
                {
                    Current = Right;
                    Direction = Right;
                }
                if (Prog.CurrentState != ProcessState.SuccessHit)
                {
                    Prog.MoveNext(Command.Resume);
                }
                else
                {
                    Prog.MoveNext(Command.Resume);
                    Prog.MoveNext(Command.Resume);
                }
            }
            
        }
        if (Prog.CurrentState == ProcessState.Damaged) // S'il il reçoit des dégâts
        {

        }

        
        if (transform.position.x > Current.position.x) // Si le Mob se trouve à droite de sa cible se retourne vers elle
        {
            transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x < Current.position.x ) // Idem pour la gauche
        {
            transform.localScale = new Vector3(-2, transform.localScale.y, transform.localScale.z);
        }

    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f); 
        if(Prog.CurrentState == ProcessState.DetectSmth) // S'il a détecté sa cible effectue un changement d'état vers "AttackSmth"
                                                         // puis relache un point de retour 
        {
            Prog.MoveNext(Command.Attack); // Changement de DetectSmth à AttackSmth
            Point.transform.parent = null; // Lâche le point de ralliement 
        }
        else if (Prog.CurrentState == ProcessState.SuccessHit) // Après avoir touché sa cible il cherche à revenir sans a avoir à détecter
                                                               // qui que se soit puis procède à un changement d'état
        {
            Prog.MoveNext(Command.Resume); // Changement de SucessHit à NoDetect
        }
        else // Si l'état n'est pas dans ce concerné en haut va vers un autre état
        { Prog.MoveNext(Command.End); } // Changement d'état de :
                                        // AttackSmth -> NoDetect = pas possible car il commence par un stopcoroutine et ne relance jamais de Wait (),
                                        // Damaged -> AttackSmth = ne se fait pas pour l'instant vu que damaged pas fait ,
                                        // PROBLEME : la condition n'est jamais utilisé 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Prog.CurrentState == ProcessState.AttackSmth) // S'il rentre en contact avec le joueur procède à un changement d'état
        {
            Prog.MoveNext(Command.Resume); // Changement d'état de :
                                           // AttackSmth -> SuccesHit ça c'est l'effet princpalement voulu,
                                           // Damaged -> NoDetect Dépend de si j'ai envie faire des dégat juste par le contact,
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjPlayer")
        {
            Life -= 2;
        }
    }
}
