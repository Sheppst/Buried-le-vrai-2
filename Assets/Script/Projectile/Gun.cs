using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject Proj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            GameObject projectile = Proj;
            projectile.GetComponent<AttackObject>().Target = target;
            projectile.GetComponent<AttackObject>().Speed = 20;
            projectile.GetComponent<AttackObject>().Thrower = tag;
            projectile.GetComponent<AttackObject>().tag = "ProjPlayer";
            projectile.GetComponent<AttackObject>().TimeBeforeDestroy = 5;
            Instantiate(projectile,transform.position,Quaternion.identity);
        }
    }
}
