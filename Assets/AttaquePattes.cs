using UnityEngine;

public class AttaquePattesBehaviour : StateMachineBehaviour
{
    private Boss boss;
    private Transform player;
    private float smashRange;
    private float detectionRange;
    private bool checkSmashRange = false;

    // Appel� au d�but de l'�tat d'animation
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        player = GameObject.FindWithTag("Player").transform;
        smashRange = boss.smashRange;
        detectionRange = boss.detectionRange;
       
    }

    // Appel� � chaque frame de l'�tat d'animation
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Commence � v�rifier la port�e de smash vers la fin de l'animation
        if (stateInfo.normalizedTime >= 0.92f && !checkSmashRange)
        {
            checkSmashRange = true;
            float distanceToPlayer = Vector3.Distance(animator.transform.position, player.position);
            if (distanceToPlayer <= smashRange)
            {
                animator.SetTrigger("Smash");
            }
            else
            {
                animator.SetTrigger("Walk");
            }
        }
    }

    // Appel� � la fin de l'�tat d'animation
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss != null)
        {
            boss.StopAttaquePattes();
        }
        animator.ResetTrigger("AttaquePattes");
        animator.ResetTrigger("Smash");
        animator.ResetTrigger("Walk");
        checkSmashRange = false; // R�initialiser pour la prochaine entr�e dans cet �tat
    }

    // M�thode appel�e par un �v�nement d'animation pour commencer l'instanciation des pattes
    public void StartPattesInstantiation()
    {
        if (boss != null)
        {
            boss.StartAttaquePattes();
        }
    }
}