using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Rigidbody rb;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public LayerMask opponentLayer;

    public float maxSpeed;
    public float accelarationRate;
    public float highSteeringAngle;
    public float normalSteeringAngle;

    private Animator animator; // Animator reference
    private int isRunningHash;
    private int isSprintingHash;
    private int isReversingHash;

    private List<Transform> wayPoints;
    private float currentMaxSpeed;
    private int currentWayPoint;
    private bool hasEntered;
    private Vector3 brakeForceVector;
    private float wayPointTimer;

    void Start()
    {
        currentWayPoint = 0;
        GlobalRaceManager globalRaceManager = FindObjectOfType<GlobalRaceManager>();
        wayPoints = globalRaceManager.botWayPoints;
        currentMaxSpeed = maxSpeed;

        // Initialize animator and hashes
        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            isRunningHash = Animator.StringToHash("isRunning");
            isSprintingHash = Animator.StringToHash("isSprinting");
            isReversingHash = Animator.StringToHash("isReversing");
        }
    }
    void FixedUpdate()
    {
        HandleAcceleration();
        HandleSteering();
        UpdateAnimations(); // Update animations based on AI state
    }

    private void HandleAcceleration()
    {
        Vector3 directionToWaypoint = (wayPoints[currentWayPoint].position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, directionToWaypoint);
        float angleToWaypoint = Vector3.SignedAngle(transform.forward, directionToWaypoint, Vector3.up);
        float distanceToWaypoint = Vector3.Distance(transform.position, wayPoints[currentWayPoint].position);

        if (Mathf.Abs(angleToWaypoint) > 25f && distanceToWaypoint < 10f)
        {
            if(maxSpeed == 27f)
            {
                currentMaxSpeed = 18f;
            }else
            {
                currentMaxSpeed = 7.5f;
            }
        } else
        {
            if (maxSpeed == 27f)
            {
                currentMaxSpeed = 27f;
            }
            else
            {
                currentMaxSpeed = 10.5f;
            }
        }

        if (rb.velocity.magnitude < currentMaxSpeed)
        {
            if (dotProduct < 0f)
            {
                brakeForceVector = -rb.velocity.normalized * (maxSpeed == 27f ? 12f : 6f);
                rb.AddForce(brakeForceVector, ForceMode.Acceleration);
            }
            else
            {
                if (rb.velocity.magnitude < 1f)
                {
                    rb.AddForce(directionToWaypoint * 1000, ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce(directionToWaypoint * accelarationRate, ForceMode.Acceleration);
                }
            }
        }
        else
        {
            brakeForceVector = -rb.velocity.normalized * (maxSpeed == 27f ? 7.5f : 3f);
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }

        if(maxSpeed == 27f)
        {
           rb.AddForce(-transform.up * 3000 * rb.velocity.magnitude);
        }
        else
        {
            rb.AddForce(-transform.up * 250 * rb.velocity.magnitude);
        }
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, 3f);
    }

    void HandleSteering()
    {
        Vector3 directionToWaypoint = (wayPoints[currentWayPoint].position - transform.position).normalized;

        float angleToWaypoint = Vector3.SignedAngle(transform.forward, directionToWaypoint, Vector3.up);

        float steeringInput = Mathf.Sign(angleToWaypoint);
        float adjustedSteeringAngle = Mathf.Lerp(normalSteeringAngle, highSteeringAngle, rb.velocity.magnitude / currentMaxSpeed) * steeringInput;

        frontLeftWheel.steerAngle = adjustedSteeringAngle;
        frontRightWheel.steerAngle = adjustedSteeringAngle;
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint") && !hasEntered)
        {
            hasEntered = true; 

            currentWayPoint++;
            if (currentWayPoint >= wayPoints.Count)
            {
                currentWayPoint = 0;
            }

            StartCoroutine(ResetTriggerFlag());
        }
    }

    private IEnumerator ResetTriggerFlag()
    {
        float delay = maxSpeed == 27f ? 0.7f : 0.45f;
        yield return new WaitForSeconds(delay); 
        hasEntered = false;
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // Determine states based on velocity and direction
        bool isMovingForward = rb.velocity.magnitude > 1f && Vector3.Dot(transform.forward, rb.velocity.normalized) > 0;
        bool isMovingBackward = rb.velocity.magnitude > 1f && Vector3.Dot(transform.forward, rb.velocity.normalized) < 0;
        bool isSprinting = isMovingForward && rb.velocity.magnitude > currentMaxSpeed * 0.8f;

        // Update animator parameters
        animator.SetBool(isRunningHash, isMovingForward && !isSprinting);
        animator.SetBool(isSprintingHash, isSprinting);
        animator.SetBool(isReversingHash, isMovingBackward);
    }
}
