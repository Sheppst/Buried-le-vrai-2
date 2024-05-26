using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Mana mana;
    [SerializeField] private int bulletCost;
    [SerializeField] private GameObject Proj;
    [SerializeField] private Transform FirePos;
    [SerializeField] private Sprite Balle;
    [SerializeField] private Animator anim;
    [SerializeField] private float SpeedBullet = 20;
    [SerializeField] private float fireRate = 0.5f; // Délai entre chaque tir en secondes
    [SerializeField] private AudioSource Audio;
    private float nextFireTime = 0f; // Temps avant le prochain tir

    private bool canShoot = true; // Booléen pour contrôler si le joueur peut tirer

    private CharacterController2D characterController;

    void Awake()
    {
        mana = GetComponent<Mana>();
        characterController = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        if (!canShoot)
            return;

        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (mana.manaPool >= bulletCost)
            {
                Audio.Play();
                mana.SpendMana(bulletCost);
                anim.SetTrigger("IsShootting");
                Shoot(target);
                nextFireTime = Time.time + fireRate; // Met à jour le temps avant le prochain tir
            }
        }
    }

    public void SetCanShoot(bool value)
    {
        canShoot = value;
    }

    void Shoot(Vector3 target)
    {
        // Tourner temporairement le joueur vers la direction du tir
        FlipTowards(target);

        GameObject projectile = Instantiate(Proj, FirePos.position, Quaternion.identity);
        projectile.transform.localScale = Vector3.one * 0.042198f;
        projectile.GetComponent<AttackObject>().Target = target;
        projectile.GetComponent<AttackObject>().Speed = SpeedBullet;
        projectile.GetComponent<AttackObject>().Thrower = tag;
        projectile.GetComponent<AttackObject>().tag = "ProjPlayer";
        projectile.GetComponent<AttackObject>().TimeBeforeDestroy = 5;
        projectile.GetComponent<AttackObject>().ObjSprite = Balle;

        // Revenir à la direction de course après un court délai
        StartCoroutine(ResetOrientation());
    }

    void FlipTowards(Vector3 target)
    {
        if (target.x > transform.position.x && !characterController.IsFacingRight())
        {
            characterController.Move(0, false, false, false, 0); // Forcer le flip
        }
        else if (target.x < transform.position.x && characterController.IsFacingRight())
        {
            characterController.Move(0, false, false, false, 0); // Forcer le flip
        }
    }

    IEnumerator ResetOrientation()
    {
        yield return new WaitForSeconds(0.1f); // Attendre un court délai
        characterController.Move(0, false, false, false, 0); // Remettre dans la direction de course
    }
}
