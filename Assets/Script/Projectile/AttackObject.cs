using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    [HideInInspector]public string TargetObject;
    [HideInInspector]public string Thrower;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public float TimeBeforeDestroy;
    public Vector3 Target;
    public float Speed = 1;
    public Sprite ObjSprite;
    // Start is called before the first frame update
    void Start()
    {
        
        spriteRenderer.sprite = ObjSprite; // D�finis le sprite en fonction de son utilisation
        Vector3 target = (Target - transform.position ); // Cr�e un vecteur entre le point d�part et le point d'arriv� puis le transforme en direction
        float dist = Vector3.Distance(transform.position, Target);
        //dist /= 10;
        //print(dist);
        //Speed -= dist;
        print(target.magnitude);
        rigid.velocity = new Vector2(target.x,target.y).normalized * Speed; // Applique cette direction � un certaine vitesse sur la v�locit� objet
        TurnOnTarget(); // Dirge le sens de pointage de l'objet en direction voulu
        Destroy(gameObject, TimeBeforeDestroy); // D�truit le GO apr�s un certains temps
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != Thrower && collision.tag != "Trigger") 
        {
            Destroy(gameObject);
        }
    }
    private void TurnOnTarget()
    {
        Vector2 directionToTarget = Target - transform.position;// Trouver la direction de la cible par rapport � la position de l'objet

        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg; // Calculer l'angle en degr�s entre la direction et la droite vers le haut (vecteur (0, 1))
                                                                                             // Cr�er une rotation � partir de cet angle
        Quaternion rotationToTarget = Quaternion.Euler(0, 0, angle + 90); // +90 degr�s pour ajuster le pointage de la rotation vers le bas sinon n�gatif pour vers le haut

        transform.rotation = rotationToTarget;// Appliquer la rotation � l'objet
    }
}
