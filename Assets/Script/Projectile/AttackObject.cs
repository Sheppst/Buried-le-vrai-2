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
        spriteRenderer.sprite = ObjSprite;
        Vector3 target = (Target - transform.position ).normalized;
        rigid.velocity = target * Speed;
        TurnOnTarget();
        Destroy(gameObject, TimeBeforeDestroy);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != Thrower) 
        {
            Destroy(gameObject);
        }
    }
    private void TurnOnTarget()
    {
        Vector2 directionToTarget = Target - transform.position;// Trouver la direction de la cible par rapport à la position de l'objet

        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg; // Calculer l'angle en degrés entre la direction et la droite vers le haut (vecteur (0, 1))
                                                                                             // Créer une rotation à partir de cet angle
        Quaternion rotationToTarget = Quaternion.Euler(0, 0, angle + 90); // +90 degrés pour ajuster le pointage de la rotation vers le bas sinon négatif pour vers le haut

        transform.rotation = rotationToTarget;// Appliquer la rotation à l'objet
    }
}
