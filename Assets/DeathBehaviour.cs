using UnityEngine;
using UnityEngine.Animations;

public class DeathBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Boss boss = animator.GetComponent<Boss>();
        if (boss != null && boss.doors != null)
        {
            foreach (var door in boss.doors)
            {
                door.isDoorOpen = true;
            }
        }

        // Destroy the boss game object
        GameObject.Destroy(animator.gameObject);
    }
}