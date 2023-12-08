using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGMovement : MonoBehaviour
{
    [SerializeField] float movementForce = 5f;
    [SerializeField] float jumpForce = 10f;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();   
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A))
            rb.AddRelativeForce(-transform.right * movementForce * Time.deltaTime,ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.S))
            rb.AddRelativeForce(-transform.forward * movementForce * Time.deltaTime, ForceMode.VelocityChange);
        if(Input.GetKey(KeyCode.D))
            rb.AddRelativeForce(transform.right * movementForce * Time.deltaTime, ForceMode.VelocityChange);
        if(Input.GetKey(KeyCode.W))
            rb.AddRelativeForce(transform.forward * movementForce * Time.deltaTime, ForceMode.VelocityChange);

        if(Input.GetKeyDown(KeyCode.Space))
            rb.AddRelativeForce(transform.up * jumpForce, ForceMode.VelocityChange);

        if(Input.GetKeyDown(KeyCode.LeftControl))
                rb.AddRelativeForce(transform.up * jumpForce * -1, ForceMode.VelocityChange);
    }
}
