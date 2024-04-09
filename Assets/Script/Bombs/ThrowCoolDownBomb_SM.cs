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
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player"); // Cherche l'objet nommé Player dans la scène et l'assigne à mon objet Player 
        Prog = new Process(); // Créer un nouveau process de ma machine d'état
        if (Prog.CurrentState == ProcessState.Inactive) // Etat de base
        {
            if (Player.transform.localScale.x > 0) // Si le joueur est dans sa scale positif 
            {
              Vector2 direction = Player.transform.right * throwingspeed; // direction prend comme paramètres l'axe X du joueur et le multiplie par une puissance donné
              direction.y = 300; // Puis direction obtient une hauteur cible
              rigid.AddForce(direction); // par la suite, on donne la force écrite dans direction à l'objet
              Prog.MoveNext(Command.Begin); // Et on passe à l'état suivant
            }
            else // Si le joueur n'est dans pas sa scale positif 
            {
                Vector2 direction = -Player.transform.right * throwingspeed;
                direction.y = 300;
                rigid.AddForce(direction);
                Prog.MoveNext(Command.Begin);
            }
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
        if (collision != null && Prog.CurrentState == ProcessState.Active && collision.gameObject.tag != "ExploFalse")
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
        yield return new WaitForSeconds(1);
        Prog.MoveNext(Command.Exit);
        Destroy(gameObject);
    }
    private IEnumerator CetteFonctionNeDevraisPasExister()
    {
        yield return new WaitForSeconds(0.0000001f);
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }
}
