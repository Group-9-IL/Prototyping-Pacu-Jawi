using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float maxBoostSpeed;
    public float minTurnSpeed;
    public float maxTurnSpeed;
    public float maxStamina;
    public float currentStamina;
    public float boostSpeed;
    public float staminaDeplation;
    public float staminaRefill;
    public float driftTurnSpeed;
    public float bonusDriftSpeed;

    public Rigidbody rb;
    public Transform cameraTransform;

    private float moveInput;
    private float turnInput;
    private float driftInput;
    private bool boostInput;
    private float turnSpeed;
    private int cartDirection;
    private bool isBoosting;
    private Vector3 initialCamera;
    private Vector3 targetCamera;
    private bool isDrifting;
    private int driftDirection;
    private float scaleDriftInput;
    private float driftTime;
    private float bonusDriftTime;
    private bool isBonusDrift;

    void Start()
    {
        currentStamina = maxStamina;
        initialCamera = new Vector3(0f, 1.5f, -2f);
        targetCamera = new Vector3(0f, 1.6f, -2.5f);
    }

    void Update()
    {
        turnInput = Input.GetAxis("Horizontal");
        moveInput = Input.GetAxisRaw("Vertical");
        driftInput = Input.GetAxisRaw("Jump");

        cartDirection = moveInput < 0 ? -1 : 1;

        boostInput = Input.GetKey(KeyCode.LeftShift);

        if(boostInput)
        {   
            if(currentStamina > 20)
            {
                isBoosting = true;
            } 

            if(currentStamina == 0)
            {
                isBoosting = false;
            }
        } else
        {
            isBoosting = false;
        }

        if(isBoosting)
        {
            currentStamina -= Time.deltaTime * staminaDeplation;
            cameraTransform.localPosition = targetCamera;
        }else {
            cameraTransform.localPosition = initialCamera;
            currentStamina += Time.deltaTime * staminaRefill;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if(!isDrifting && driftInput > 0 && rb.velocity.magnitude > 7.5)
        {
            isDrifting = true;
            driftDirection = (int)Mathf.Ceil(turnInput);
            driftTime = 0;
        }

        if(isDrifting)
        {
            driftTime += Time.deltaTime;
        }

        if(isDrifting && (driftInput <= 0 || rb.velocity.magnitude < 7.5))
        {
            isDrifting = false;

            if(driftTime > 2)
            {
                bonusDriftTime = 0;
                isBonusDrift = true;
            }
        }

        if (isBonusDrift && bonusDriftTime < 3)
        {
            bonusDriftTime += Time.deltaTime;
        } else
        {
            isBonusDrift = false;
        }
    }

    void FixedUpdate()
    {

        if(isBonusDrift)
        {
            if(rb.velocity.magnitude < maxBoostSpeed)
            {
                rb.AddForce(transform.forward * moveInput * bonusDriftSpeed * acceleration);
            }
        }

        if(isBoosting)
        {   
            if (rb.velocity.magnitude < maxBoostSpeed)
            {
                rb.AddForce(transform.forward * moveInput * acceleration * boostSpeed);
            }
        } else
        {
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(transform.forward * moveInput * acceleration);
            }
        }
        
        float turnSpeed = Mathf.Lerp(minTurnSpeed, maxTurnSpeed, rb.velocity.magnitude / maxSpeed);

        if (isDrifting)
        {
            scaleDriftInput = GetScaleDrift();
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0f, turnSpeed * driftDirection * driftTurnSpeed * scaleDriftInput, 0f)));
        }else
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0f, turnSpeed * cartDirection * turnInput, 0f)));
        }

    }

    float GetScaleDrift()
    {
        if (driftDirection < 0)
        {
            if (turnInput < 0)
            {
                return 1f;
            } else if (turnInput == 0)
            {
                return 0.66f;
            } else
            {
                return 0.33f;
            }
        } else
        {
            if (turnInput > 0)
            {
                return 1f;
            } else if (turnInput == 0)
            {
                return 0.66f;
            } else
            {
                return 0.33f;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gachaBox"))
        {
            GachaBox gachaBoxInstance = other.gameObject.GetComponent<GachaBox>();

            if (gachaBoxInstance != null)
            {
                gachaBoxInstance.OpenBox();
            }
            else
            {
                Debug.LogError("GachaBox component not found on the collided object.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Mud"))
        {
            Debug.Log("kena");
        }
    }

}
