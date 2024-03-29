using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SM_Throw;

public class ThrowCoolDownBomb_SM : MonoBehaviour
{
    Process Prog;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private CircleCollider2D collid;
    [SerializeField] private float throwingspeed;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Prog = new Process();
        if (Prog.CurrentState == ProcessState.Inactive)
        {
            Vector2 direction = Player.transform.right * throwingspeed;
            direction.y = 300;
            rigid.AddForce(direction);
            Prog.MoveNext(Command.Begin);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Prog.CurrentState == ProcessState.Terminated)
        {
            StopAllCoroutines();
        }
        if (Prog.CurrentState == ProcessState.Paused)
        {
            StartCoroutine(CetteFonctionNeDevraisPasExister());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && Prog.CurrentState == ProcessState.Active)
        {
            Prog.MoveNext(Command.Pause);
            rigid.bodyType = RigidbodyType2D.Static;
            collid.isTrigger = true;
            collid.radius = 4;
            gameObject.tag = "ExploThrTrue";
            if (Prog.CurrentState == ProcessState.Paused)
            {
                StartCoroutine(Throwexplosion());
            }
        }
    }
    private IEnumerator Throwexplosion()
    {
        yield return new WaitForSeconds(3);
        Prog.MoveNext(Command.Exit);
        Destroy(gameObject);
    }
    private IEnumerator CetteFonctionNeDevraisPasExister()
    {
        yield return new WaitForSeconds(0.0000001f);
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }
}
