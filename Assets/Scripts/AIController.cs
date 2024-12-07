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

    private List<Transform> wayPoints;
    private int currentWayPoint;
    private bool hasEntered = false;

    void Start()
    {
        currentWayPoint = 0;
        GlobalRaceManager globalRaceManager = FindObjectOfType<GlobalRaceManager>();
        wayPoints = globalRaceManager.botWayPoints;
    }

    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
    }

    void FixedUpdate()
    {
        HandleAcceleration();
        HandleSteering();
    }

    private void HandleAcceleration()
    {
        Vector3 directionToWaypoint = (wayPoints[currentWayPoint].position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, directionToWaypoint);
        float angleToWaypoint = Vector3.SignedAngle(transform.forward, directionToWaypoint, Vector3.up);
        float distanceToWaypoint = Vector3.Distance(transform.position, wayPoints[currentWayPoint].position);

        if (Mathf.Abs(angleToWaypoint) > 25f)
        {
            maxSpeed = 18f;
        } else
        {
            maxSpeed = 27.5f;
        }

        if (rb.velocity.magnitude < maxSpeed)
        {
            if (dotProduct < 0f) 
            {
                Vector3 brakeForceVector = -rb.velocity.normalized * 12f;
                rb.AddForce(brakeForceVector, ForceMode.Acceleration);
            }
            else
            {
                if (rb.velocity.magnitude < 1f)
                {
                    rb.AddForce(directionToWaypoint * 300, ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce(directionToWaypoint * accelarationRate, ForceMode.Acceleration);
                }
            }
        } else
        {
            Vector3 brakeForceVector = -rb.velocity.normalized * 7.5f;
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }

        rb.AddForce(-transform.up * 3000 * rb.velocity.magnitude);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, 3f);
    }

    void HandleSteering()
    {
        Vector3 directionToWaypoint = (wayPoints[currentWayPoint].position - transform.position).normalized;


        float angleToWaypoint = Vector3.SignedAngle(transform.forward, directionToWaypoint, Vector3.up);

        float steeringInput = Mathf.Sign(angleToWaypoint);
        float adjustedSteeringAngle = Mathf.Lerp(normalSteeringAngle, highSteeringAngle, rb.velocity.magnitude / maxSpeed) * steeringInput;

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
        yield return new WaitForSeconds(0.3f); 
        hasEntered = false;
    }
}
