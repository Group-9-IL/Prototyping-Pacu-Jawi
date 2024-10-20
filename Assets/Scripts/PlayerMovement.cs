using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public JawiStats jawiStats;
    public Rigidbody rb;
    public GameObject item;
    public Transform cameraTransform;
    private Animator dropItem;
    public GameObject itemUI;
    // public GameObject getItem;

    private float moveInput;
    private float turnInput;
    private float driftInput;
    private bool boostInput;
    private float turnSpeed;
    private float currentStamina;
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
    private bool hasItem = false;

    void Start()
    {
        currentStamina = jawiStats.maxStamina;
        initialCamera = new Vector3(0f, 1.5f, -2f);
        targetCamera = new Vector3(0f, 1.6f, -2.5f);
        dropItem = item.GetComponent<Animator>();
        // getItem = item.GetComponent<Animator>();
    }   

    void Update()
    {
        HandleInput();
        HandleBoost();
        HandleDrift();
        UsingItem();        
    }

    void FixedUpdate()
    {

        FixedMovement();
        FixedRotation();
    }

    void HandleInput()
    {
        turnInput = Input.GetAxisRaw("Horizontal");
        moveInput = Input.GetAxisRaw("Vertical");
        driftInput = Input.GetAxisRaw("Jump");
        boostInput = Input.GetKey(KeyCode.LeftShift);
        cartDirection = moveInput < 0 ? -1 : 1;

    }

    void HandleBoost()
    {
        if (boostInput && moveInput > 0)
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
            currentStamina -= Time.deltaTime * jawiStats.staminaDeplation;
            cameraTransform.localPosition = targetCamera;
        }
        else
        {
            cameraTransform.localPosition = initialCamera;
            currentStamina += Time.deltaTime * jawiStats.staminaRefill;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, jawiStats.maxStamina);
    }

    void HandleDrift()
    {
        if (!isDrifting && driftInput > 0 && rb.velocity.magnitude > 7.5 && turnInput != 0 && moveInput > 0)
        {
            isDrifting = true;
            driftDirection = (int)Mathf.Ceil(turnInput);
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

    void FixedMovement()
    {
        if (isBonusDrift)
        {
            if (rb.velocity.magnitude < jawiStats.maxBoostSpeed)
            {
                rb.AddForce(transform.forward * moveInput * jawiStats.bonusDriftSpeed * jawiStats.acceleration);
            }
        }

        if (isBoosting)
        {
            if (rb.velocity.magnitude < jawiStats.maxBoostSpeed)
            {
                rb.AddForce(transform.forward * moveInput * jawiStats.acceleration * jawiStats.boostSpeed);
            }
        }
        else
        {
            if (rb.velocity.magnitude < jawiStats.maxSpeed)
            {
                rb.AddForce(transform.forward * moveInput * jawiStats.acceleration);
            }
        }
    }

    void FixedRotation()
    {
        float turnSpeed = Mathf.Lerp(jawiStats.minTurnSpeed, jawiStats.maxTurnSpeed, rb.velocity.magnitude / jawiStats.maxSpeed);

        if (isDrifting)
        {
            scaleDriftInput = GetScaleDrift();
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0f, turnSpeed * driftDirection * jawiStats.driftTurnSpeed * scaleDriftInput, 0f)));
        }
        else
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

            if (gachaBoxInstance != null && !hasItem )
            {
                gachaBoxInstance.OpenBox();
                hasItem = true;
                dropItem.SetBool("hasItemAnim",true);
                // itemUI.SetActive(true);
            }
            else
            {
                Debug.LogError("GachaBox component not found on the collided object.");
            }
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
    private void UsingItem(){
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // buat fungsi agar item bisa digunakan
            hasItem = false;
        }
    }
}
