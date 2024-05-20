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
        // Pas d'action sp�cifique pendant l'�tat Idle
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            // V�rifier la distance au joueur pour d�cider de l'action suivante
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

        // R�initialiser les triggers pour �viter les conflits
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Smash");
        animator.ResetTrigger("AttaquePattes");
    }
}