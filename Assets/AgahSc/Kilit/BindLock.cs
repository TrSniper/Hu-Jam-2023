using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindLock : MonoBehaviour, ILock
{
    [SerializeField]BindLockType type;
    Rigidbody rb;

    public bool isBound = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void GetUnlocked() // use to unlock
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //if has the bindlock key
    }
    */

}