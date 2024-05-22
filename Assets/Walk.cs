using UnityEngine;

public class WalkBehaviour : StateMachineBehaviour
{
    private Transform player;
    private Boss boss;
    private float detectionRange;
    private float smashRange;
    private float moveSpeed;
    private float walkDuration = 0.3f; // Dur�e de marche en secondes
    private float walkTimer;
    private bool checkSmashRange = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        boss = animator.GetComponent<Boss>();
        detectionRange = boss.detectionRange;
        smashRange = boss.smashRange;
        moveSpeed = boss.walkSpeed; // Obtenir la vitesse de d�placement depuis le script principal
        walkTimer = 0f;
        checkSmashRange = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            Vector3 direction = (player.position - animator.transform.position).normalized;
            direction.y = 0; // Assurer que le mouvement reste horizontal

            // V�rifie si le boss est � la limite de sa zone de mouvement
            if (!boss.isAtLimit)
            {
                animator.transform.position += direction * moveSpeed * Time.deltaTime;
            }

            walkTimer += Time.deltaTime;
            if (walkTimer >= walkDuration || boss.isAtLimit)
            {
                // Commence � v�rifier la port�e de smash vers la fin de l'animation ou si le boss atteint la limite
                if ((stateInfo.normalizedTime >= 0.98f && !checkSmashRange) || boss.isAtLimit)
                {
                    checkSmashRange = true;
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
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // R�initialiser les triggers utilis�s pour �viter les conflits
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("AttaquePattes");
        animator.ResetTrigger("Smash");
        animator.ResetTrigger("Idle");
        checkSmashRange = false; // R�initialiser pour la prochaine entr�e dans cet �tat
    }
}
