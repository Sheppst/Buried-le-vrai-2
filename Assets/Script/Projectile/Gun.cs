using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        mana = GetComponent<Mana>();
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
        Vector3 NewSight = transform.localScale;
        if (transform.position.x - target.x < 0)
        {
            NewSight.x = Mathf.Abs(NewSight.x);
        }
        else if (transform.position.x - target.x > 0)
        {
            NewSight.x = -Mathf.Abs(NewSight.x);
        }
        transform.localScale = NewSight;

        GameObject projectile = Instantiate(Proj, FirePos.position, Quaternion.identity);
        projectile.transform.localScale = Vector3.one * 0.042198f;
        projectile.GetComponent<AttackObject>().Target = target;
        projectile.GetComponent<AttackObject>().Speed = SpeedBullet;
        projectile.GetComponent<AttackObject>().Thrower = tag;
        projectile.GetComponent<AttackObject>().tag = "ProjPlayer";
        projectile.GetComponent<AttackObject>().TimeBeforeDestroy = 5;
        projectile.GetComponent<AttackObject>().ObjSprite = Balle;
    }

}
