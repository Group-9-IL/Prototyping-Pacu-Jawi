using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) // Example: press W to run
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
    }
}
