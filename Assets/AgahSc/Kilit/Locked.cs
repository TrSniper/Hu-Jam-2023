using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : MonoBehaviour
{
    private ILock @lock;

    private void Start()
    {
        @lock = GetComponent<ILock>();
        Debug.Log(@lock);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == 7)) return;
        if (other.gameObject.GetComponent<PlayerKeyManager>().HasKey)
        {
            @lock.GetUnlocked();
        }
    }
}
