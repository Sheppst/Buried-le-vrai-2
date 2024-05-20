using UnityEngine;

public class SmashBehaviour : StateMachineBehaviour
{
    private bool hasSetTrigger;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasSetTrigger = false; // Réinitialiser l'indicateur
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               
            // Assurer que la transition ne se fait qu'une seule fois
            animator.SetTrigger("Idle");
          
       
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Smash");
    }
}

