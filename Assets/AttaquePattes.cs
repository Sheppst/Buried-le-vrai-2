using UnityEngine;

public class AttaquePattesBehaviour : StateMachineBehaviour
{
    private Boss boss;
    private Transform player;
    private float smashRange;
    private float detectionRange;
    private bool checkSmashRange = false;

    // Appelé au début de l'état d'animation
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        player = GameObject.FindWithTag("Player").transform;
        smashRange = boss.smashRange;
        detectionRange = boss.detectionRange;
       
    }

    // Appelé à chaque frame de l'état d'animation
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Commence à vérifier la portée de smash vers la fin de l'animation
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

    // Appelé à la fin de l'état d'animation
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss != null)
        {
            boss.StopAttaquePattes();
        }
        animator.ResetTrigger("AttaquePattes");
        animator.ResetTrigger("Smash");
        animator.ResetTrigger("Walk");
        checkSmashRange = false; // Réinitialiser pour la prochaine entrée dans cet état
    }

    // Méthode appelée par un événement d'animation pour commencer l'instanciation des pattes
    public void StartPattesInstantiation()
    {
        if (boss != null)
        {
            boss.StartAttaquePattes();
        }
    }
}