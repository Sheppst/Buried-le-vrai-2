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
    [SerializeField] private float Life;
    private float radius = 4.5f;
    private bool CoDec = true;
    private bool Pass = true;
    private bool Restart = true;
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
        Left.parent = null;
        Right.parent = null;
        Prog = new Process();
        CS = Prog.CurrentState; // Pour le débug
        if (Prog.CurrentState == ProcessState.Inactive)
        {
            Current = Left;
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
        //ProcessState CSN = Prog.CurrentState;   //Pour le débug
        //if (CS != CSN) 
        //{
        //    print(CSN.ToString());
        //    CS = CSN;
        //}  //Pour le débug

        if (Prog.CurrentState == ProcessState.Terminated)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
        if (Prog.CurrentState == ProcessState.Moved)
        {
            rigid.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Current.position.x, transform.position.y, Current.position.z), speed / 1000);
            if (transform.position == Current.position)
            {
                if (Current == Left)
                {
                    Current = Right;
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
                else if (Current == Right)
                {
                    Current = Left;
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
            }

        }
        if (Prog.CurrentState == ProcessState.DetectSmth)
        {
            if (CoDec)
            {
                StartCoroutine(Detected());
                CoDec = false;
            }
        }
        if (Prog.CurrentState == ProcessState.AttackSmth)
        {
            if (cold)
            {
                GameObject AttObj = Instantiate(AttackObject, transform.position, Quaternion.identity);
                AttObj.GetComponent<AttackObject>().Target = HitPlayer;
                AttObj.GetComponent<AttackObject>().TargetObject = HitPlayerTag;
                AttObj.GetComponent<AttackObject>().Thrower = gameObject.tag;
                StartCoroutine(Cooldown());
                cold = false;
            }
        }
        if (Prog.CurrentState == ProcessState.NoDetect)
        {
            if (Return)
            {
                if (Current == Left)
                {
                    Current = Right;
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
                else if (Current == Right)
                {
                    Current = Left;
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
                Return = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, AttackObject.transform.position, speed / 1000);
            if (transform.position == AttackObject.transform.position)
            {
                AttackObject.transform.SetParent(gameObject.transform);
                Prog.MoveNext(Command.Resume);
                Return = true;
            }
        }
        if (Prog.CurrentState == ProcessState.SuccessHit)
        {

            transform.position = Vector3.MoveTowards(transform.position, AttackObject.transform.position, speed / 1000);
            if (Restart)
            {
                StopAllCoroutines();
                if (Current == Left)
                {
                    Current = Right;
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
                else if (Current == Right)
                {
                    Current = Left;
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
                StartCoroutine(Wait());
                Return = false;
                Restart = false;
                if (transform.position == AttackObject.transform.position)
                {
                    StopAllCoroutines();
                    Prog.MoveNext(Command.End);
                }
            }
        }
        if (Prog.CurrentState == ProcessState.Damaged)
        {
            if (dmg)
            {

                StopAllCoroutines();
                rigid.velocity = Vector3.zero;
                StartCoroutine(Wait());
                dmg = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (Prog.CurrentState == ProcessState.Moved)
            {
                Return = false;
            }
            Prog.MoveNext(Command.Hit);
        }

        Pass = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ZoneDetection.position, radius, WhoHaveToBeAggro);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)// S'il y a détection du joueur
            {
                Pass = false;
                if (colliders[i].tag == "Player")
                {
                    HitPlayer = colliders[i].transform.position;
                    HitPlayerTag = colliders[i].tag;
                }
                Vector3 P = ZoneDetection.position;
                Debug.DrawLine(P, colliders[i].transform.position);
                if (Prog.CurrentState != ProcessState.DetectSmth && Prog.CurrentState != ProcessState.AttackSmth && Prog.CurrentState != ProcessState.SuccessHit && Prog.CurrentState != ProcessState.Damaged)
                {
                    StopAllCoroutines();
                    Prog.MoveNext(Command.Detect);
                }
            }
        }
        if (Pass)
        {
            if (Prog.CurrentState == ProcessState.DetectSmth || Prog.CurrentState == ProcessState.AttackSmth)
            {
                StopAllCoroutines();
                CoDec = true;
                rigid.velocity = Vector3.zero;
                Prog.MoveNext(Command.End);
            }
        }

    }
    private IEnumerator Detected()
    {
        yield return new WaitForSeconds(1f);
        if (Prog.CurrentState == ProcessState.DetectSmth)
        {
            Prog.MoveNext(Command.Attack);
        }
        CoDec = true;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Prog.MoveNext(Command.Resume);
        Restart = true;
        dmg = true;
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        cold = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Prog.MoveNext(Command.Resume);
        }
    }
}
