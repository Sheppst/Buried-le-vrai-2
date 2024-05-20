using UnityEngine;

public class AttaquePattesBehaviour : StateMachineBehaviour
{
    private Boss boss;

    // Appelé au début de l'état d'animation
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
    }

    // Appelé à chaque frame de l'état d'animation
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // Appelé à la fin de l'état d'animation
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss != null)
        {
            boss.StopAttaquePattes();
        }
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
