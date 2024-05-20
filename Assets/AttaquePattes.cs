using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Actions � effectuer lorsque l'�tat AttaquePattes est atteint
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Logic for updating AttaquePattes state if necessary
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Actions � effectuer lors de la sortie de l'�tat AttaquePattes
        animator.SetTrigger("Walk"); // Apr�s AttaquePattes, passer � l'�tat de marche
    }
}