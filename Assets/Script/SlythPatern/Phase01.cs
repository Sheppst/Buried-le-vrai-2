using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SM_Phase01;
using UnityEngine.SceneManagement;

public class Phase01 : MonoBehaviour
{
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;
    [SerializeField] private string NomDuJoueur;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject Ray;
    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject Wall;
    [SerializeField] private float ChargeStrength = 1 ;
    [SerializeField] private float ChargeSpeed;
    [SerializeField] private Animator BossAnim;
    private AudioSource MainAudio;
    private GameObject Bl;
    private Vector3 Newpos;
    private Vector2 Vec2Pos;
    private Rigidbody2D rigid;
    private float Life;
    private float speed = 5f;
    private float initialSpeed;
    private bool Flip;
    private Transform Player;
    private PolygonCollider2D colid;
    public Transform Current;
    Process Prog;
    ProcessState CS;
    // Start is called before the first frame update
    void Start()
    {
        initialSpeed = speed;
        Vec2Pos = transform.position;
        Bl = null;
        //Life = transform.parent.gameObject.GetComponent<AllMovement>().Life;
        Life = 1000;
        colid = GetComponent<PolygonCollider2D>();
        MainAudio = GetComponentInParent<AudioSource>();
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
        Vec2Pos = transform.position;
        //Life = transform.parent.gameObject.GetComponent<AllMovement>().Life;
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
            transform.parent.gameObject.SetActive(false);
            SceneManager.LoadScene(0);
        }
        if (Prog.CurrentState == ProcessState.Inactive) //Etat de reset du boss 
        {
            speed = initialSpeed;
            if (transform.position.x > Player.position.x) // Si le joueur se trouve � gauche ...
            {
                Current = Left; // ... L'objectif de d�placement devient la gauche
                Ray.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180); //... Le raycast se tourne vers la gauche 
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z); //... Le boss se retourne vers le joueur 
                
            }
            else if (transform.position.x < Player.position.x) // Vice-versa
            {
                Current = Right;
                Ray.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                
            }
            StartCoroutine(Wait()); // Lance un petit cooldown, puis change d'�tat
        }
        if (Prog.CurrentState == ProcessState.ChoisingPhase) // Le boss choisit son patern...
        {
            StopAllCoroutines(); // pr�caution
            float distC = Vector3.Distance(Current.position, transform.position); // Distance entre la position du boss et de sa destination
            float distP = Vector3.Distance(Player.position, transform.position); // Distance entre la position du boss et le joueur 
            if (distC/2 > distP) // ... si le joueur est proche,
                                 // car la distance entre la position entre le boss et son objectif reste sup�rieur � la distance
                                 // avec le joueur malgr� le fait qu'elle soit divis� par deux 
            {
                Prog.MoveNext(Command.Bit); // Changement d'�tat de ChoisingPhase -> Bited
                Newpos =  new Vector3(Player.position.x, transform.position.y, transform.position.z); // Dernier endroit que le boss � vu le joueur
                Bl = Instantiate(Block,Newpos,Quaternion.identity);
                Ray.SetActive(true); // L'objet tenant la d�tection s'active                                                            
            }
            else if (distC/2 < distP) // ...si le joueur est trop loin
            {
                MainAudio.Play();
                Prog.MoveNext(Command.CutPhase); // Changement sp�cial pour rendre dans les temps, ChoisingPhase -> Charge
            }
        }
        if (Prog.CurrentState == ProcessState.Bited) // Quand le boss cherche � mordre le joueur // Priorite 2
        {
            BossAnim.SetBool("Walk", true);
            BossAnim.SetBool("Charge", false);
            if (Ray.activeSelf == false) // Si l'objet de d�tection n'est pas actif...
            {
                if (Bl != null) // Vide Bl
                {
                    Bl.GetComponent<BlockBit>().After(); // D�truit l'actuel Bl
                    Bl = null; // Et d�finis Bl comme vide
                }
                Prog.MoveNext(Command.CutPhase); // ... Changement d'�tat de Bited -> Inactive 
            }
            else if (!Ray.GetComponentInChildren<DetectBite>().Detect) // 1�re condition d�tecte si qlq chose rentre en colision avec l'objet de d�tection
            {
                transform.position = Vector3.MoveTowards(transform.position, Newpos, speed * Time.deltaTime); // D�place le boss vers la derni�re position du joueur connu
            }
        }
        if (Prog.CurrentState == ProcessState.Charge) // Quand le boss cherche � chager le joueur 
        {

            speed = initialSpeed * ChargeSpeed;
            BossAnim.SetBool("Charge", true);
            BossAnim.SetBool("Walk", false);
            Vector3 cur = new Vector3(Current.position.x, transform.position.y, transform.position.z); // prend uniquement la position de l'objectif en X 
            Vector2 target = (cur - transform.position).normalized; // Calcule un vecteur de direction entre la position du boss et de l'objectif en restant sur l'axe X
            rigid.velocity = target * speed; // Lance le vecteur � une certaine vitesse sur la v�locit�
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
        yield return new WaitForSeconds(1.5f);
        if (Prog.CurrentState == ProcessState.Inactive) // Temps d'attente pour le cooldown en Inactive
        {
            Prog.MoveNext(Command.Begin); // Changement d'�tat entre Inactive -> ChoicePhase
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ProjPlayer") // Prend effet si le boss rentre au collision avec un objet pouvant l'endomager 
        {
            Life -= 10;
            print("touch�"); // Pour les tests futures
        }
        if (Prog.CurrentState == ProcessState.Charge) // Si durant la phase de Charge ...
        {
            Vector2 YProp = new Vector2(collision.transform.position.x, collision.transform.position.y) * Vector2.one * 500;
            Vector2 CollidKnock = (YProp - Vec2Pos).normalized;
            if (collision.tag == "Player")
            {
                GameObject.Find("Player").GetComponent<PlayerLifeSystem>().ChargeByBoss();
                //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(CollidKnock * ChargeStrength);
                BossAnim.SetBool("Charge", false);
            }
            if (collision != null && collision.gameObject.tag != "ProjPlayer" ) // /... Le boss rencontre n'importe quel obstacle, alors ...
            {
                rigid.velocity = Vector2.zero; // ... Le boss arr�te de bouger
                BossAnim.SetBool("Charge", false);
                Prog.MoveNext(Command.CutPhase); // ... Effectue une transition d'�tat de Charge -> Inactive
            }
        }
        else if (collision.tag == "BossObject" && Prog.CurrentState == ProcessState.Bited)
        {
            Ray.SetActive(false);
        }
    }
    public Vector3 PositionJoueur ()
    {
        return Newpos;
    }
    public bool Finis()
    {
        return true;
    }
}
