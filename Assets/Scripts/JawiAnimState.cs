using UnityEngine;

public class JawiAnimState : MonoBehaviour
{
    Animator animator;
    int isRunningHash;
    int isReversingHash;
    int isSprintingHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isReversingHash = Animator.StringToHash("isReversing");
        isSprintingHash = Animator.StringToHash("isSprinting");
    }

    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isReversing = animator.GetBool(isReversingHash);
        bool isSprinting = animator.GetBool(isSprintingHash);

        bool forwardPressed = Input.GetKey("w");
        bool reversePressed = Input.GetKey("s");
        bool sprintPressed = Input.GetKey("left shift");

        if (!isRunning && forwardPressed) // Example: press W to run
        {
            animator.SetBool(isRunningHash, true);
        }
        if (isRunning && !forwardPressed)
        {
            animator.SetBool(isRunningHash, false);
        }

        if (!isReversing && reversePressed)
        {
            animator.SetBool(isReversingHash, true);
        }
        if (isReversing && !reversePressed)
        {
            animator.SetBool(isReversingHash, false);
        }

        if (!isSprinting && (forwardPressed && sprintPressed))
        {
            animator.SetBool(isSprintingHash, true);
        }
        if (isSprinting && (!forwardPressed || !sprintPressed))
        {
            animator.SetBool(isSprintingHash, false);
        }
    }
}
