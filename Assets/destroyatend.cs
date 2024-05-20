using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patte : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Patte prefab.");
        }
    }

    // Cette fonction sera appelée par un Animation Event à la fin de l'animation
    public void DestroyAtEndOfAnimation()
    {
        Destroy(gameObject);
    }
}
