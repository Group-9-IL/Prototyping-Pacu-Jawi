using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator animator;
    int isRunningHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool forwardPressed = Input.GetKey("w");
        if (!isRunning && forwardPressed) // Example: press W to run
        {
            animator.SetBool(isRunningHash, true);
        }
        if (isRunning && !forwardPressed)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}
