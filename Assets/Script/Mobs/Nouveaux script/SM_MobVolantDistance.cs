using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobVolant_SM;

public class SM_MobVolantDistance : MonoBehaviour
{
    [SerializeField] private Transform ZoneDetection;
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;
    [SerializeField] private Rigidbody2D rigid;
    private Transform Current;
    [SerializeField] private LayerMask WhoHaveToBeAggro;
    [SerializeField] private GameObject AttackObject;
    [SerializeField] private float speed;
    private float Life = 10;
    private float radius = 4.5f;
    private bool CoDec = true;
    private bool Pass = true;
    //private bool Restart = true;
    private bool Return = true;
    private bool dmg = true;
    private bool cold = true;
    private Vector3 HitPlayer;
    private string HitPlayerTag;
    ProcessState CS;
    Process Prog;
    // Start is called before the first frame update
    void Start()
    {
        Left.parent = null; // D�tache les points de patrouilles
        Right.parent = null; // D�tache les points de patrouilles
        Prog = new Process(); // Lance la machine d'�tat
        CS = Prog.CurrentState; // Pour le d�bug
        if (Prog.CurrentState == ProcessState.Inactive) // Phase initiale
        {
            Current = Left; // L'objectif
            Prog.MoveNext(Command.Begin);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (Prog.CurrentState == ProcessState.Damaged) //Pour le d�bug
        //{
        //    print(D.ToString());
        //}
        ProcessState CSN = Prog.CurrentState;   //Pour le d�bug
        if (CS != CSN) 
        {
            print(CSN.ToString());
            CS = CSN;
        }  //Pour le d�bug
        if (Life <= 0) // Si la vie du monstre est inf�rieur ou �gale � z�ro lance la proc�dure de mort du monstre
        {
            Prog.MoveNext(Command.Death); // Changement d'�tat de n'importe quel autre �tat � Terminated
        }
        if (Prog.CurrentState == ProcessState.Terminated) // Etat de mort du monstre
        {
            StopAllCoroutines(); // Pr�caution
            Right.parent = gameObject.transform;
            Left.parent = gameObject.transform;
            this.enabled = false;
            Destroy(gameObject); // Destruction du monstre
        }
        if (Prog.CurrentState == ProcessState.Moved) // Etat de mouvement de patrouille du monstre
        {
            // rigid.velocity = Vector3.zero; // Variable servant au script du monstre a�rien de contact inutile dans ce script
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Current.position.x, transform.position.y, Current.position.z), (speed * Time.deltaTime) / 10) ; 
            //^
            //|
            //Script de mouvement du monstre vers un objectif donn�e (Current) 
            if (transform.position == Current.position) // Si le monstre arrive � destination de son objectif alors ...
            {
                if (Current == Left) // ... Si l'objectf est la gauche, le nouvelle objectif est � droite et retourne le monstre dans le sens de l'objectif
                {
                    Current = Right;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
                }
                else if (Current == Right) // ... Si l'objectf est la droite, le nouvelle objectif est � gauche et retourne le monstre dans le sens de l'objectif
                {
                    Current = Left;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }

        }
        if (Prog.CurrentState == ProcessState.DetectSmth) // Si le cercle de d�tection d�tecte quelque chose,
                                                          // patiente qlq sec puis en fonction de l'�tat de la d�tection,
                                                          // d�clenche ou non l'�tat d'attaque sinon retourne � celui de mouvemnt
        {
            if (CoDec) // Condition qui permet de ne lancer la coroutine qu'une seule fois 
            {
                StartCoroutine(Detected());
                CoDec = false;
            }
        }
        if (Prog.CurrentState == ProcessState.AttackSmth) // Etat d'attaque
        {
            if (cold) // Si le cooldown entre les projectiles est respect� alors envoie un projectile en direction de la cible puis repasse en attente
            {
                GameObject AttObj = Instantiate(AttackObject, transform.position, Quaternion.identity); // Instantiation dans un objet pour y changer ses param�tres
                AttObj.GetComponent<AttackObject>().Target = HitPlayer; // D�finis la cible du projectile
                AttObj.GetComponent<AttackObject>().TargetObject = HitPlayerTag; // Donne le tag de la cible
                AttObj.GetComponent<AttackObject>().TimeBeforeDestroy = 15;
                AttObj.GetComponent<AttackObject>().tag = tag; 
                AttObj.GetComponent<AttackObject>().Speed = 2;
                AttObj.GetComponent<AttackObject>().Thrower = gameObject.tag; // Donne le tag du lanceur,
                                                                              // pour �viter que le projectile disparaisse aussi vite qu'il appara�t
                
                StartCoroutine(Cooldown()); // Lance le tenmp d'attente
                cold = false; // Met la variable d'acc�s � la condition : Off
            }
        }
        if (Prog.CurrentState == ProcessState.NoDetect) // Etat de non d�tection de la cible
        {
            StopAllCoroutines(); // Pr�caution
            if (Return) // Condition qui fait changer de direction le monstre apr�s sa d�tection une seule fois, 
                        // PROBLEME : Ne passe en "true" que durant la d�claration, �tant une variable priv� PB
            {
                if (Current == Left) // Si l'objectif �tais � gauche � effectue une transition sur la droite
                {
                    Current = Right;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
                }
                else if (Current == Right) // Si l'objectif �tais � droite � effectue une transition sur la gauche
                {
                    Current = Left;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                Return = false; //Ferme la condition pour ne l'effectuer qu'une seule fois 
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Current.position.x, transform.position.y, Current.position.z), (speed * Time.deltaTime)/10); // Le d�place � son nouvel objectif
            if (Current.position.x == transform.position.x) // Si l'objectif est atteint...
            {
                Prog.MoveNext(Command.Resume); // Relance le mob en Moved
                Return = true; // R�active la variable pour le prochain passage
                cold = true;
            }
        }
        {
            //if (Prog.CurrentState == ProcessState.SuccessHit) // Normalement inutile dans le cadre du monstre qui attaque � distance 
            //{
            //    if (Restart)
            //    {
            //        StopAllCoroutines();
            //        if (Current == Left)
            //        {
            //            Current = Right;
            //            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            //        }
            //        else if (Current == Right)
            //        {
            //            Current = Left;
            //            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            //        }
            //        StartCoroutine(Wait());
            //        Return = false;
            //        Restart = false;
            //        if (transform.position == AttackObject.transform.position)
            //        {
            //            StopAllCoroutines();
            //            Prog.MoveNext(Command.End);
            //        }
            //    }
            //}
        }
        if (Prog.CurrentState == ProcessState.Damaged) // Etat o� le monstre se prend un d�g�t
        {
            if (dmg)
            {
                StopAllCoroutines(); // Pr�caution
                // rigid.velocity = Vector3.zero; // Inutile dans le cas du Mob Distance
                StartCoroutine(Wait()); // Attend un peu avant de reprendre 
                dmg = false; // Emp�che la condition de se r�effectuer
            }
        }
        if (Input.GetKeyDown(KeyCode.H)) // C'est ici que les d�g�ts sont enclench�, par test sinon d�sactivation
        {
            if (Prog.CurrentState == ProcessState.Moved)
            {
                Return = false; // Pour ne pas r�effectuer un changement de direction dans ce cas de transition sp�ciale
            }
            Prog.MoveNext(Command.Hit); // Transition de n'importe quel �tat � Damaged
        }

        Pass = true; // Cette variable va permettre de savoir quand il N'Y as PAS de d�tection
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ZoneDetection.position, radius, WhoHaveToBeAggro); //Cr�e une liste de tout objet rentrons en collision avec le cercle de d�tection
        for (int i = 0; i < colliders.Length; i++) // Puis fais une boucle qui va check chacun des �lements aper�u,
                                                   // les param�tres sont que i est un "int" d�marrant � 0,
                                                   // qui ne peut tourner que si i est inf�rieur au nombre d'objet d�tect� dans la liste,
                                                   // et � chaque passage de boucle i augmente de 1
                                                   // Probl�me (Resolu) : si il n'y a aucune d�tection rien ne l'avertit
        {
            if (colliders[i].gameObject != gameObject)// S'il y a d�tection du joueur
            {
                Pass = false; // La variable qui permettait de savoir si aucune d�tection n'a �t� faite est donc mis en : OFF
                if (colliders[i].tag == "Player") // Si l'un des objets poss�de le tag Joueur alors ... 
                {
                    HitPlayer = colliders[i].transform.position; // La variable de target devient la position de cet objet
                    HitPlayerTag = colliders[i].tag; // Et on r�cup�re son tag
                }
                Vector3 P = ZoneDetection.position; // Position centrale de la zone de d�tection 
                Debug.DrawLine(P, colliders[i].transform.position); // Dessine un trait pour voir d'o� la d�tection se fait dans l'�diteur
                if (Prog.CurrentState != ProcessState.DetectSmth && Prog.CurrentState != ProcessState.AttackSmth && Prog.CurrentState != ProcessState.SuccessHit && Prog.CurrentState != ProcessState.Damaged)
                // Si l'�tat actuel se d�crit comme passif
                {
                    StopAllCoroutines(); // Pr�caution
                    Prog.MoveNext(Command.Detect); // Envoie dans l'�tat DetectSmth
                }
            }
        }
        if (Pass) // Si la variable est vraie alors aucune d�tection n'est faite et ainsi
        {
            if (Prog.CurrentState == ProcessState.DetectSmth || Prog.CurrentState == ProcessState.AttackSmth)
            // Si le monstre est en train de d�tecter ou d'attaquer, alors stop la d�tection et repasse en un �tat passif 
            {
                StopAllCoroutines(); // Pr�caution
                CoDec = true; // R�active la variable permettant d'acc�der � la d�tection dans DetectSmth
                //rigid.velocity = Vector3.zero; // inutile dans le cadre de l'ennemie � distance
                Prog.MoveNext(Command.End); // Si dans :
                                            // DetectSmth -> Moved
                                            // AttackSmth -> NoDetect
            }
        }

    }
    private IEnumerator Detected()
    {
        yield return new WaitForSeconds(1f);
        if (Prog.CurrentState == ProcessState.DetectSmth)
        {
            Prog.MoveNext(Command.Attack); // Si la d�tection est toujours d'actualit� alors passe de DetectSmth -> AttackSmth
        }
        CoDec = true; // R�active la variable permettant d'acc�der � la d�tection dans DetectSmth
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        if (Prog.CurrentState == ProcessState.Damaged) // Permet de cr�er un l�ger stop caus� par le d�g�t re�u
        {
            Prog.MoveNext(Command.Resume); // Puis ram�ne de l'�tat Damaged -> NoDetect
            dmg = true; //Remet la variable de condition de l'�tat de Damaged � "true"
            // Restart = true; // Inutile dans le contexte du mob distance 
        }
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        cold = true; // Red�marre la variable permettant de re-attaquer
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == "ProjPlayer" )
        {
            Life -= 2;
            ReceiveDamage();
        }
    }
    public void ReceiveDamage()
    {
        if (Prog.CurrentState == ProcessState.Moved)
        {
            Return = false;
        }
        Prog.MoveNext(Command.Hit);
    }
}
