using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    public GameObject mudPrefab;
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
    private float steeringFactor;
    private float mudLifeTime;
    private float ramForce;
    private Vector3 initialCamera;
    private Vector3 targetCamera;

    private float boostTimer;
    private float cleanRunTimer;

    void Start()
    {
        currentStamina = jawiStats.maxStamina;
        initialCamera = new Vector3(0, 5f, -5f);
        targetCamera = new Vector3(0, 4.5f, -6f);
        steeringFactor = 1f;
        HandleCamera();
    }   

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 15f, Color.red, 1f);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5f, opponentLayer)){
            Debug.Log("kena");
            Debug.Log("Hit without LayerMask: " + hit.collider.name);
        }
        if (!TimerManager.Instance.getIsGameStarted())
        {
            rb.velocity = Vector3.zero;
            return;
        }

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
            cleanRunTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()  
    {
        HandleAcceleration();
        HandleSteering();
        HandleBraking();
    }

    private void HandleBraking()
    {
        if (Input.GetKey(KeyCode.B))
        {
            Vector3 brakeForceVector = -rb.velocity.normalized * 7.5f;
            rb.AddForce(brakeForceVector, ForceMode.Acceleration);
        }
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
            }else if(isBoosting || boostTimer > 0f)
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

    private void SetWheelFriction(float forwardStiffness, float sidewaysStiffness)
    {
        WheelFrictionCurve forwardFriction = frontLeftWheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = frontLeftWheel.sidewaysFriction;

        forwardFriction.stiffness = forwardStiffness;
        sidewaysFriction.stiffness = sidewaysStiffness;

        frontLeftWheel.forwardFriction = forwardFriction;
        frontRightWheel.forwardFriction = forwardFriction;
        frontLeftWheel.sidewaysFriction = sidewaysFriction;
        frontRightWheel.sidewaysFriction = sidewaysFriction;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Mud") && cleanRunTimer <= 0f)
        {
            SetWheelFriction(1.2f, 2.4f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mud"))
        {
            SetWheelFriction(2f, 6f);
        }
    }

    public void ItemMud()
    {
        Vector3 spawnPosition = transform.position - transform.forward * 2;
        spawnPosition.y = transform.position.y - 0.55f;

        mudLifeTime = 7f;

        Quaternion playerRotation = transform.rotation;
    
        Quaternion mudRotation = Quaternion.Euler(playerRotation.eulerAngles.x, 0, 0);
        GameObject mudInstance = Instantiate(mudPrefab, spawnPosition, mudRotation);

        Destroy(mudInstance, mudLifeTime);
    }

    public void ItemBoost()
    {
        boostTimer = 3f;
        Debug.Log("Make Boost");
    }

    public void ItemCleanRun()
    {
        cleanRunTimer = 7f;
        Debug.Log("Make Clean Run");
    }

    public void ItemRam()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red, 1f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f, opponentLayer))
        {
            Debug.Log("Kena Musuh 1");
            Rigidbody opponentRb = hit.collider.GetComponent<Rigidbody>();
            if (opponentRb != null)
            {
                Debug.Log("Kena Musuh 2");
                Vector3 sidewaysForce = transform.right * 500;
                opponentRb.AddForce(sidewaysForce, ForceMode.Impulse);
            }
        }
    }
}
