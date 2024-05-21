using UnityEngine;

public class AuraBehaviour : StateMachineBehaviour
{
    private Boss boss;
    private GameObject auraInstance;
   

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();

        // Activer l'aura
        if (boss.auraPrefab != null && boss.auraSpawnPoint != null)
        {
            auraInstance = Instantiate(boss.auraPrefab, boss.auraSpawnPoint.position, boss.auraSpawnPoint.rotation);
            boss.isAuraActive = true;
            
        }
        else
        {
            Debug.LogError("�chec de l'instanciation de l'aura : le prefab ou le point de spawn est null");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Si la sant� du boss est en dessous du seuil et que l'aura n'a pas encore �t� utilis�e
       
        
            animator.SetTrigger("AttaquePattes");

        
          
        

            
                  
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // D�sactiver l'aura
        if (auraInstance != null)
        {
            Destroy(auraInstance);
            boss.isAuraActive = false;
        }
    }
}