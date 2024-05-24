using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour
{
    // OnStateEnter est appelé lorsque la transition commence et que l'état de la machine à états commence à évaluer cet état
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
    }
}
    