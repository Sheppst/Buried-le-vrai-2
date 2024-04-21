using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpAndDown : StateMachineBehaviour
{
    [SerializeField] Aria Aria;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Aria = GameObject.FindGameObjectWithTag("Aria").GetComponent<Aria>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Aria.AttackUpAndDownState();
    }


}