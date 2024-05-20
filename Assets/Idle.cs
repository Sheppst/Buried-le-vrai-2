using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private Transform player;
    private Boss boss;
    private float detectionRange;
    private float smashRange;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        boss = animator.GetComponent<Boss>();
        detectionRange = boss.detectionRange;
        smashRange = boss.smashRange;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Pas d'action spécifique pendant l'état Idle
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
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
                animator.SetTrigger("Walk");
            }
        }
        else
        {
            animator.SetTrigger("Walk");
        }

        // Réinitialiser les triggers pour éviter les conflits
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Smash");
        animator.ResetTrigger("AttaquePattes");
    }
}