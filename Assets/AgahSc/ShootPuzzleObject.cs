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
    [SerializeField] LayerMask bulletLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void MovePuzzleObject() // move object based on direction
    {
        rb.velocity = Vector3.zero; transform.rotation = Quaternion.identity;//first stop it
        //rb.Sleep(); same thing?
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
        Debug.Log(collision);
        if(collision.gameObject.layer != bulletLayer) return;
        MovePuzzleObject();
        Debug.Log("Object is supposed to move");
    }

}
