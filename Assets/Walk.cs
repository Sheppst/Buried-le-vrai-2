using UnityEngine;

public class WalkBehaviour : StateMachineBehaviour
{
    private Transform player;
    private Boss boss;
    private float detectionRange;
    private float smashRange;
    private float moveSpeed;
    private float walkDuration = 0.3f; // Durée de marche en secondes
    private float walkTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        boss = animator.GetComponent<Boss>();
        detectionRange = boss.detectionRange;
        smashRange = boss.smashRange;
        moveSpeed = boss.walkSpeed; // Obtenir la vitesse de déplacement depuis le script principal
        walkTimer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            Vector3 direction = (player.position - animator.transform.position).normalized;
            direction.y = 0; // Assurer que le mouvement reste horizontal
            animator.transform.position += direction * moveSpeed * Time.deltaTime;

            walkTimer += Time.deltaTime;
            if (walkTimer >= walkDuration)
            {
                // Vérifier la distance au joueur pour décider de l'action suivante
                float distanceToPlayer = Vector3.Distance(animator.transform.position, player.position);
                if (distanceToPlayer <= smashRange)
                {
                    animator.SetTrigger("Smash");
                }
                else if (distanceToPlayer <= detectionRange)
                {
                    animator.SetTrigger("AttaquePattes");
                }
                else
                {
                    animator.SetTrigger("Idle");
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Réinitialiser les triggers utilisés pour éviter les conflits
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("AttaquePattes");
    }
}