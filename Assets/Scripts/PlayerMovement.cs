using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public JawiStats jawiStats;
    public Rigidbody rb;
    public Camera mainCamera;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public LayerMask opponentLayer;

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
    private Vector3 initialCamera;
    private Vector3 targetCamera;

    private float boostTimer;
    private float cleanRunTimer;

    void Start()
    {
        currentStamina = jawiStats.maxStamina;
        initialCamera = new Vector3(0, 5f, -5f);
        targetCamera = new Vector3(0, 4.5f, -6f);
        HandleCamera();
    }   

    void Update()
    {   

        accelarationInput = Input.GetAxisRaw("Vertical");
        steeringInput = Input.GetAxisRaw("Horizontal");
        boostInput = Input.GetKey(KeyCode.LeftShift);
        driftInput = Input.GetAxisRaw("Jump");

        HandleCamera();
        HandleBoost();
        HandleDrift();

        if(boostTimer > 0f)
        {
            boostTimer -= Time.deltaTime;
        }

        if (cleanRunTimer > 0f)
        {
            rb.drag = 0f;
            rb.angularDrag = 0.3f;
            cleanRunTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()  
    {
        HandleAcceleration();
        HandleSteering();
    }

    private void HandleAcceleration()
    {
        if (accelarationInput < 1f && Vector3.Dot(rb.velocity, transform.forward) > 0)
        {
            Vector3 brakeForceVector = -rb.velocity.normalized * 14f;
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }

        if (accelarationInput > 1f && Vector3.Dot(rb.velocity, transform.forward) < 0)
        {
            Vector3 brakeForceVector = rb.velocity.normalized * 14f;
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }

        if (accelarationInput != 0f)
        {

            if (rb.velocity.magnitude < 1f)
            {
                rb.AddForce(transform.forward * accelarationInput * jawiStats.initialAccelaration, ForceMode.Acceleration);
            }

            if (isBonusDrift)
            {
                if (rb.velocity.magnitude < jawiStats.maxBoostSpeed)
                {
                    rb.AddForce(transform.forward * accelarationInput * jawiStats.boostAccelarationRate, ForceMode.Acceleration);
                }
            }
            else if (isBoosting || boostTimer > 0f)
            {
                if (rb.velocity.magnitude < jawiStats.maxBoostSpeed)
                {
                    rb.AddForce(transform.forward * accelarationInput * jawiStats.boostAccelarationRate, ForceMode.Acceleration);
                }
            }
            else
            {
                if (rb.velocity.magnitude < jawiStats.maxSpeed)
                {
                    rb.AddForce(transform.forward * accelarationInput * jawiStats.accelarationRate, ForceMode.Acceleration);
                }
            }
        } else
        {
            Vector3 brakeForceVector = -rb.velocity.normalized * 3f;
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }

        rb.AddForce(-transform.up * jawiStats.downForce * rb.velocity.magnitude);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, 3f);
    }

    private void HandleSteering()
    {
        float adjustedSteeringAngle = isDrifting
            ? jawiStats.driftSteeringAngle * driftDirection * GetScaleDrift()
            : Mathf.Lerp(jawiStats.normalSteeringAngle, jawiStats.highSteeringAngle, rb.velocity.magnitude / jawiStats.maxSpeed) * steeringInput;

        currentSteeringAngle = adjustedSteeringAngle;

        frontLeftWheel.steerAngle = adjustedSteeringAngle;
        frontRightWheel.steerAngle = adjustedSteeringAngle;
    }

    private void HandleBoost()
    {
        if (boostInput && accelarationInput > 0)
        {
            if (currentStamina > 20)
            {
                isBoosting = true;
            }

            if (currentStamina == 0)
            {
                isBoosting = false;
            }
        }
        else
        {
            isBoosting = false;
        }

        if (isBoosting)
        {
            currentStamina -= Time.deltaTime * jawiStats.staminaDepletion;
        }
        else
        {
            currentStamina += Time.deltaTime * jawiStats.staminaRefill;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, jawiStats.maxStamina);
    }

    private void HandleDrift()
    {
        if (!isDrifting && driftInput > 0 && rb.velocity.magnitude > 7.5 && steeringInput != 0 && accelarationInput > 0)
        {
            isDrifting = true;
            driftDirection = (int)Mathf.Ceil(steeringInput);
            driftTime = 0;
        }

        if (isDrifting)
        {
            driftTime += Time.deltaTime;
        }

        if (isDrifting && (driftInput <= 0 || rb.velocity.magnitude < 7.5))
        {
            isDrifting = false;

            if (driftTime > 2)
            {
                bonusDriftTime = 0;
                isBonusDrift = true;
            }
        }

        if (isBonusDrift && bonusDriftTime < 3)
        {
            bonusDriftTime += Time.deltaTime;
        }
        else
        {
            isBonusDrift = false;
        }
    }

    private void HandleCamera()
    {
        if(isBoosting || isBonusDrift || boostTimer > 0f)
        {
            mainCamera.transform.localPosition = targetCamera;
        }
        else
        {
            mainCamera.transform.localPosition = initialCamera;
        }
    }

    private float GetScaleDrift()
    {
        if (driftDirection < 0)
        {
            if (steeringInput < 0)
            {
                return 1f;
            }
            else if (steeringInput == 0)
            {
                return 0.9f;
            }
            else
            {
                return 0.8f;
            }
        }
        else
        {
            if (steeringInput > 0)
            {
                return 1f;
            }
            else if (steeringInput == 0)
            {
                return 0.9f;
            }
            else
            {
                return 0.8f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mud") && cleanRunTimer <= 0f)
        {
            rb.drag = 1.6f;
            rb.angularDrag = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mud"))
        {
            rb.drag = 0f;
            rb.angularDrag = 0.3f;
        }
    }

    public void ItemBoost()
    {
        boostTimer = 3f;
    }

    public void ItemCleanRun()
    {
        cleanRunTimer = 7f;
    }

    public void ItemRam()
    { 
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 7f, opponentLayer))
        {
            Transform parent = hit.collider.transform.parent;
            Rigidbody opponentRb = parent.transform.parent.GetComponent<Rigidbody>();
            GameObject opponentGameObject = parent.transform.parent.gameObject;
            if (opponentRb != null)
            {
                StartCoroutine(ApplyStun(opponentRb, opponentGameObject));
            }
        }
    }

    private IEnumerator ApplyStun(Rigidbody opponentRb, GameObject opponentGameObject)
    {
        float stunDuration = 2f;

        while (stunDuration > 0)
        {
            if (opponentRb.velocity.magnitude > 2f)
            {
                Vector3 decelerationForce = -opponentRb.velocity.normalized * 100f;
                opponentRb.AddForce(decelerationForce, ForceMode.Impulse);
            }
            stunDuration -= Time.deltaTime;
            yield return null;
        }
    }
}
