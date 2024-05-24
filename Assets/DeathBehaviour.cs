using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour
{
    // OnStateEnter est appel� lorsque la transition commence et que l'�tat de la machine � �tats commence � �valuer cet �tat
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
    