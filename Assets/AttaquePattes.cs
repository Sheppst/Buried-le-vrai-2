using UnityEngine;

public class AttaquePattesBehaviour : StateMachineBehaviour
{
    private Boss boss;

    // Appel� au d�but de l'�tat d'animation
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
    }

    // Appel� � chaque frame de l'�tat d'animation
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // Appel� � la fin de l'�tat d'animation
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss != null)
        {
            boss.StopAttaquePattes();
        }
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
