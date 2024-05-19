using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogError("Animator non trouvé sur " + gameObject.name);
        }
    }
}