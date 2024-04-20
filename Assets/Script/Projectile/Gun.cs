using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject Proj;
    [SerializeField] private Transform FirePos;
    [SerializeField] private Sprite Balle;
    [SerializeField] private Animator anim;
    [SerializeField] private float SpeedBullet = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButton(0)) 
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
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            
            anim.SetTrigger("IsShootting");
            GameObject projectile = Proj;
            projectile.transform.localScale = Vector3.one * 0.042198f;
            projectile.GetComponent<AttackObject>().Target = target;
            projectile.GetComponent<AttackObject>().Speed = SpeedBullet;
            projectile.GetComponent<AttackObject>().Thrower = tag;
            projectile.GetComponent<AttackObject>().tag = "ProjPlayer";
            projectile.GetComponent<AttackObject>().TimeBeforeDestroy = 5;
            projectile.GetComponent<AttackObject>().ObjSprite = Balle;
            Instantiate(projectile,FirePos.position,Quaternion.identity);
        }
    }
}
