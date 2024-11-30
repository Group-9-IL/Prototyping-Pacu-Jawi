using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public JawiStats jawiStats;
    public Rigidbody rb;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public LayerMask opponentLayer;

    private List<Transform> wayPoints;
    private int currentWayPoint;
    private float accelarationInput;
    private float steeringInput;
    private float driftInput;
    private bool boostInput;
    private bool isBoosting;
    private bool isDrifting;
    private float bonusDriftTime;
    private bool isBonusDrift;
    private float currentSteeringAngle;
    private float currentStamina;
    private int driftDirection;
    private float driftTime;


    void Start()
    {
        currentWayPoint = 0;
        GlobalRaceManager globalRaceManager = FindObjectOfType<GlobalRaceManager>();
        wayPoints = globalRaceManager.botWayPoints;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        HandleAcceleration();
        HandleSteering();
    }

    private void HandleAcceleration()
    {
        // Get the direction to the next waypoint
        Vector3 directionToWaypoint = (wayPoints[currentWayPoint].position - transform.position).normalized;

        // Calculate the dot product to determine if the AI is moving forward
        float dotProduct = Vector3.Dot(rb.velocity, transform.forward);

        // If moving in the opposite direction, apply brake force to slow down
        if (dotProduct < 0)
        {
            Vector3 brakeForceVector = rb.velocity.normalized * 14f;
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }

        // When AI is not accelerating or braking, apply basic force to keep the car moving
        if (accelarationInput != 0f)
        {
            // If the car is at low speed, apply initial acceleration
            if (rb.velocity.magnitude < 1f)
            {
                rb.AddForce(transform.forward * accelarationInput * jawiStats.initialAccelaration, ForceMode.Acceleration);
            }

            // Apply regular acceleration based on the car's speed
            else
            {
                if (rb.velocity.magnitude < jawiStats.maxSpeed)
                {
                    rb.AddForce(transform.forward * accelarationInput * jawiStats.accelarationRate, ForceMode.Acceleration);
                }
            }
        }
        else
        {
            // Apply braking force when there is no acceleration input
            Vector3 brakeForceVector = -rb.velocity.normalized * 3f;
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }

        // Add downforce to stabilize the car at high speeds
        rb.AddForce(-transform.up * jawiStats.downForce * rb.velocity.magnitude);

        // Limit angular velocity to avoid spinning too fast
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, 3f);
    }

    private void HandleSteering()
    {
        // Get the direction to the next waypoint
        Vector3 directionToWaypoint = (wayPoints[currentWayPoint].position - transform.position).normalized;

        // Calculate the angle needed to turn towards the waypoint
        float targetAngle = Vector3.SignedAngle(transform.forward, directionToWaypoint, transform.up);

        // Adjust steering based on speed and whether the car is drifting
        float adjustedSteeringAngle =  Mathf.Lerp(jawiStats.normalSteeringAngle, jawiStats.highSteeringAngle, rb.velocity.magnitude / jawiStats.maxSpeed) * Mathf.Sign(targetAngle);

        // Apply the adjusted steering angle to the car's wheels
        currentSteeringAngle = adjustedSteeringAngle;

        frontLeftWheel.steerAngle = adjustedSteeringAngle;
        frontRightWheel.steerAngle = adjustedSteeringAngle;

        // If the AI car reaches the waypoint, switch to the next one
        if (Vector3.Distance(transform.position, wayPoints[currentWayPoint].position) < 5f)
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.Count;  // Loop through waypoints
        }
    }
}
