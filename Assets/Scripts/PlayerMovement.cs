using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public JawiStats jawiStats;
    public Rigidbody rb;
    public Camera mainCamera;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

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

    void Start()
    {
        currentStamina = jawiStats.maxStamina;
        initialCamera = new Vector3(0, 5f, -5f);
        targetCamera = new Vector3(0, 4.5f, -6f);
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
    }

    void FixedUpdate()  
    {
        HandleAcceleration();
        HandleSteering();
    }

    private void HandleAcceleration()
    {

        if (accelarationInput != 0f)
        {

            if (rb.velocity.magnitude < 1f)
            {
                rb.AddForce(transform.forward * accelarationInput * jawiStats.initialAccelaration, ForceMode.Acceleration);
            }

            if(isBonusDrift)
            {
                if(rb.velocity.magnitude < jawiStats.maxBoostSpeed)
                {
                    rb.AddForce(transform.forward * accelarationInput * jawiStats.boostAccelarationRate, ForceMode.Acceleration);
                }
            }else if(isBoosting)
            {
                if (rb.velocity.magnitude < jawiStats.maxBoostSpeed)
                {
                    rb.AddForce(transform.forward * accelarationInput * jawiStats.boostAccelarationRate, ForceMode.Acceleration);
                }
            }else
            {
                if (rb.velocity.magnitude < jawiStats.maxSpeed)
                {
                    rb.AddForce(transform.forward * accelarationInput * jawiStats.accelarationRate, ForceMode.Acceleration);
                }
            }


        }

        rb.AddForce(-transform.up * jawiStats.downForce * rb.velocity.magnitude);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, 3f);
    }
        
    private void HandleSteering()
    {

        currentSteeringAngle = isDrifting
            ? jawiStats.driftSteeringAngle * driftDirection * GetScaleDrift()
            : Mathf.Lerp(jawiStats.normalSteeringAngle, jawiStats.highSteeringAngle, rb.velocity.magnitude / jawiStats.maxSpeed) * steeringInput;

        frontLeftWheel.steerAngle = currentSteeringAngle;
        frontRightWheel.steerAngle = currentSteeringAngle;
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
        if(isBoosting || isBonusDrift)
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
        if (other.gameObject.CompareTag("gachaBox"))
        {
            GachaBox gachaBoxInstance = other.gameObject.GetComponent<GachaBox>();

            gachaBoxInstance.OpenBox();
            
        }

        if (other.gameObject.CompareTag("Mud"))
        {
            rb.drag = 1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mud"))
        {
            rb.drag = 0.2f;
        }
    }
}
