using UnityEngine;

public class SmashBehaviour : StateMachineBehaviour
{
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               
            // Assurer que la transition ne se fait qu'une seule fois
            animator.SetTrigger("Walk");
          
       
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Smash");
    }
}

