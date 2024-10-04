using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float acceleration = 10f;
    public float maxSpeed = 15f;
    public float slowTurnSpeed = 0.5f;
    public float highTurnSpeed = 0.75f;

    public Rigidbody rb;
    public Transform cameraTransform;

    private float moveInput;
    private float turnInput;
    private float turnSpeed;
    void Start()
    {
        cameraTransform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 2);
    }

    void Update()
    {
        turnInput = Input.GetAxis("Horizontal");
        moveInput = Input.GetAxis("Vertical");

        turnSpeed = rb.velocity.z < maxSpeed *  50 / 100 ? slowTurnSpeed : highTurnSpeed;

    }

    void FixedUpdate()
    {
        
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveInput * acceleration);
        }

        if (rb.velocity.magnitude > 0.5f)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0f, turnSpeed * turnInput, 0f)));
        }
            
    }
}
