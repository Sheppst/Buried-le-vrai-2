using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Actions à effectuer lorsque l'état AttaquePattes est atteint
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Logic for updating AttaquePattes state if necessary
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Actions à effectuer lors de la sortie de l'état AttaquePattes
        animator.SetTrigger("Walk"); // Après AttaquePattes, passer à l'état de marche
    }
}