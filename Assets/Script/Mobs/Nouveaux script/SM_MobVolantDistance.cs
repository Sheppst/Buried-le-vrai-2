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
        Left.parent = null; // Détache les points de patrouilles
        Right.parent = null; // Détache les points de patrouilles
        Prog = new Process(); // Lance la machine d'état
        CS = Prog.CurrentState; // Pour le débug
        if (Prog.CurrentState == ProcessState.Inactive) // Phase initiale
        {
            Current = Left; // L'objectif
            Prog.MoveNext(Command.Begin);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (Prog.CurrentState == ProcessState.Damaged) //Pour le débug
        //{
        //    print(D.ToString());
        //}
        ProcessState CSN = Prog.CurrentState;   //Pour le débug
        if (CS != CSN) 
        {
            print(CSN.ToString());
            CS = CSN;
        }  //Pour le débug
        if (Life <= 0) // Si la vie du monstre est inférieur ou égale à zéro lance la procédure de mort du monstre
        {
            Prog.MoveNext(Command.Death); // Changement d'état de n'importe quel autre état à Terminated
        }
        if (Prog.CurrentState == ProcessState.Terminated) // Etat de mort du monstre
        {
            StopAllCoroutines(); // Précaution
            Right.parent = gameObject.transform;
            Left.parent = gameObject.transform;
            this.enabled = false;
            Destroy(gameObject); // Destruction du monstre
        }
        if (Prog.CurrentState == ProcessState.Moved) // Etat de mouvement de patrouille du monstre
        {
            // rigid.velocity = Vector3.zero; // Variable servant au script du monstre aérien de contact inutile dans ce script
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Current.position.x, transform.position.y, Current.position.z), (speed * Time.deltaTime) / 10) ; 
            //^
            //|
            //Script de mouvement du monstre vers un objectif donnée (Current) 
            if (transform.position == Current.position) // Si le monstre arrive à destination de son objectif alors ...
            {
                if (Current == Left) // ... Si l'objectf est la gauche, le nouvelle objectif est à droite et retourne le monstre dans le sens de l'objectif
                {
                    Current = Right;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
                }
                else if (Current == Right) // ... Si l'objectf est la droite, le nouvelle objectif est à gauche et retourne le monstre dans le sens de l'objectif
                {
                    Current = Left;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }

        }
        if (Prog.CurrentState == ProcessState.DetectSmth) // Si le cercle de détection détecte quelque chose,
                                                          // patiente qlq sec puis en fonction de l'état de la détection,
                                                          // déclenche ou non l'état d'attaque sinon retourne à celui de mouvemnt
        {
            if (CoDec) // Condition qui permet de ne lancer la coroutine qu'une seule fois 
            {
                StartCoroutine(Detected());
                CoDec = false;
            }
        }
        if (Prog.CurrentState == ProcessState.AttackSmth) // Etat d'attaque
        {
            if (cold) // Si le cooldown entre les projectiles est respecté alors envoie un projectile en direction de la cible puis repasse en attente
            {
                GameObject AttObj = Instantiate(AttackObject, transform.position, Quaternion.identity); // Instantiation dans un objet pour y changer ses paramêtres
                AttObj.GetComponent<AttackObject>().Target = HitPlayer; // Définis la cible du projectile
                AttObj.GetComponent<AttackObject>().TargetObject = HitPlayerTag; // Donne le tag de la cible
                AttObj.GetComponent<AttackObject>().TimeBeforeDestroy = 15;
                AttObj.GetComponent<AttackObject>().tag = tag; 
                AttObj.GetComponent<AttackObject>().Speed = 2;
                AttObj.GetComponent<AttackObject>().Thrower = gameObject.tag; // Donne le tag du lanceur,
                                                                              // pour éviter que le projectile disparaisse aussi vite qu'il apparaît
                
                StartCoroutine(Cooldown()); // Lance le tenmp d'attente
                cold = false; // Met la variable d'accès à la condition : Off
            }
        }
        if (Prog.CurrentState == ProcessState.NoDetect) // Etat de non détection de la cible
        {
            StopAllCoroutines(); // Précaution
            if (Return) // Condition qui fait changer de direction le monstre après sa détection une seule fois, 
                        // PROBLEME : Ne passe en "true" que durant la déclaration, étant une variable privé PB
            {
                if (Current == Left) // Si l'objectif étais à gauche à effectue une transition sur la droite
                {
                    Current = Right;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
                }
                else if (Current == Right) // Si l'objectif étais à droite à effectue une transition sur la gauche
                {
                    Current = Left;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                Return = false; //Ferme la condition pour ne l'effectuer qu'une seule fois 
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Current.position.x, transform.position.y, Current.position.z), (speed * Time.deltaTime)/10); // Le déplace à son nouvel objectif
            if (Current.position.x == transform.position.x) // Si l'objectif est atteint...
            {
                Prog.MoveNext(Command.Resume); // Relance le mob en Moved
                Return = true; // Réactive la variable pour le prochain passage
                cold = true;
            }
        }
        {
            //if (Prog.CurrentState == ProcessState.SuccessHit) // Normalement inutile dans le cadre du monstre qui attaque à distance 
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
        if (Prog.CurrentState == ProcessState.Damaged) // Etat où le monstre se prend un dégât
        {
            if (dmg)
            {
                StopAllCoroutines(); // Précaution
                // rigid.velocity = Vector3.zero; // Inutile dans le cas du Mob Distance
                StartCoroutine(Wait()); // Attend un peu avant de reprendre 
                dmg = false; // Empêche la condition de se réeffectuer
            }
        }
        if (Input.GetKeyDown(KeyCode.H)) // C'est ici que les dégâts sont enclenché, par test sinon désactivation
        {
            if (Prog.CurrentState == ProcessState.Moved)
            {
                Return = false; // Pour ne pas réeffectuer un changement de direction dans ce cas de transition spéciale
            }
            Prog.MoveNext(Command.Hit); // Transition de n'importe quel état à Damaged
        }

        Pass = true; // Cette variable va permettre de savoir quand il N'Y as PAS de détection
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ZoneDetection.position, radius, WhoHaveToBeAggro); //Crée une liste de tout objet rentrons en collision avec le cercle de détection
        for (int i = 0; i < colliders.Length; i++) // Puis fais une boucle qui va check chacun des élements aperçu,
                                                   // les paramètres sont que i est un "int" démarrant à 0,
                                                   // qui ne peut tourner que si i est inférieur au nombre d'objet détecté dans la liste,
                                                   // et à chaque passage de boucle i augmente de 1
                                                   // Problème (Resolu) : si il n'y a aucune détection rien ne l'avertit
        {
            if (colliders[i].gameObject != gameObject)// S'il y a détection du joueur
            {
                Pass = false; // La variable qui permettait de savoir si aucune détection n'a été faite est donc mis en : OFF
                if (colliders[i].tag == "Player") // Si l'un des objets possède le tag Joueur alors ... 
                {
                    HitPlayer = colliders[i].transform.position; // La variable de target devient la position de cet objet
                    HitPlayerTag = colliders[i].tag; // Et on récupère son tag
                }
                Vector3 P = ZoneDetection.position; // Position centrale de la zone de détection 
                Debug.DrawLine(P, colliders[i].transform.position); // Dessine un trait pour voir d'où la détection se fait dans l'éditeur
                if (Prog.CurrentState != ProcessState.DetectSmth && Prog.CurrentState != ProcessState.AttackSmth && Prog.CurrentState != ProcessState.SuccessHit && Prog.CurrentState != ProcessState.Damaged)
                // Si l'état actuel se décrit comme passif
                {
                    StopAllCoroutines(); // Précaution
                    Prog.MoveNext(Command.Detect); // Envoie dans l'état DetectSmth
                }
            }
        }
        if (Pass) // Si la variable est vraie alors aucune détection n'est faite et ainsi
        {
            if (Prog.CurrentState == ProcessState.DetectSmth || Prog.CurrentState == ProcessState.AttackSmth)
            // Si le monstre est en train de détecter ou d'attaquer, alors stop la détection et repasse en un état passif 
            {
                StopAllCoroutines(); // Précaution
                CoDec = true; // Réactive la variable permettant d'accéder à la détection dans DetectSmth
                //rigid.velocity = Vector3.zero; // inutile dans le cadre de l'ennemie à distance
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
            Prog.MoveNext(Command.Attack); // Si la détection est toujours d'actualité alors passe de DetectSmth -> AttackSmth
        }
        CoDec = true; // Réactive la variable permettant d'accéder à la détection dans DetectSmth
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        if (Prog.CurrentState == ProcessState.Damaged) // Permet de créer un léger stop causé par le dégât reçu
        {
            Prog.MoveNext(Command.Resume); // Puis ramène de l'état Damaged -> NoDetect
            dmg = true; //Remet la variable de condition de l'état de Damaged à "true"
            // Restart = true; // Inutile dans le contexte du mob distance 
        }
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        cold = true; // Redémarre la variable permettant de re-attaquer
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
