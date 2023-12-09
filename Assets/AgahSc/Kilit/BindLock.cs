using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindLock : MonoBehaviour, ILock
{
    [SerializeField]BindLockType type;
    [Tooltip("The Bindlocked gameObjects rigidbody must be referenced here")]
    [SerializeField]Rigidbody rb;

    [Tooltip("Don't touch...")]
    public bool isBound = true;
    private bool foundKey = false;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void GetUnlocked() // use to unlock
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.None;
        isBound = false;
        Debug.Log("BindLock Unlocked");
    }
    void CheckForBindKey() // use to check for key
    {
        foreach (var key in PlayerKeyManager.instance.bindLockKeys)
        {   
            if (key.GetKeyType() == type)
            {
                foundKey = true;
                GetUnlocked();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!foundKey) CheckForBindKey();
        if (!(other.gameObject.layer == 7))
            return;
        GetUnlocked();
    }

}