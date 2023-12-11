using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPuzzleObject : MonoBehaviour
{
    private enum MoveDir
    {
        Right, Left, Up, Down, Forward, Backward
    }
    Rigidbody rb;
    [SerializeField] MoveDir moveDir;
    [SerializeField] float forceMultiplier;
    [SerializeField] bool willUseGravity;
    [SerializeField] RigidbodyConstraints constraintsBeforeHit; 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = constraintsBeforeHit;
    }

    void MovePuzzleObject() // move object based on direction
    {
        rb.constraints = RigidbodyConstraints.None;
        switch (moveDir)
        {
            case MoveDir.Left:     rb.AddRelativeForce(-transform.right * forceMultiplier, ForceMode.VelocityChange); break;
            case MoveDir.Right:    rb.AddRelativeForce(transform.right * forceMultiplier, ForceMode.VelocityChange); break;
            case MoveDir.Up:       rb.AddRelativeForce(transform.up * forceMultiplier, ForceMode.VelocityChange); break;
            case MoveDir.Down:     rb.AddRelativeForce(-transform.up * forceMultiplier, ForceMode.VelocityChange); break;
            case MoveDir.Forward:  rb.AddRelativeForce(transform.forward * forceMultiplier, ForceMode.VelocityChange); break;
            case MoveDir.Backward: rb.AddRelativeForce(-transform.forward * forceMultiplier, ForceMode.VelocityChange); break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.tag != "Bullet") return;

        if (willUseGravity) rb.useGravity = true; else rb.useGravity = false;

        MovePuzzleObject();
        Debug.Log("Object is supposed to move");
    }
}
