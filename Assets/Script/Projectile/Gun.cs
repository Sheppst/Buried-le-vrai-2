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
    private float nextFireTime = 0f; // Temps avant le prochain tir

    void Awake()
    {
        mana = GetComponent<Mana>();
    }

    void Update()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (mana.manaPool >= bulletCost)
            {
                mana.SpendMana(bulletCost);
                anim.SetTrigger("IsShootting");
                Shoot(target);
                nextFireTime = Time.time + fireRate; // Met à jour le temps avant le prochain tir
            }
        }
    }

    void Shoot(Vector3 target)
    {
        if (transform.position.x - target.x < 0)
        {
            Vector3 NewSight = transform.localScale;
            NewSight.x = Mathf.Abs(NewSight.x);
            transform.localScale = NewSight;
        }
        else if (transform.position.x - target.x > 0)
        {
            Vector3 NewSight = transform.localScale;
            NewSight.x = Mathf.Abs(NewSight.x) * -1;
            transform.localScale = NewSight;
        }

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