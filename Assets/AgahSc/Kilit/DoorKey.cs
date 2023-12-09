using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    [SerializeField] DoorLockType doorLockType;

    public DoorLockType GetKeyType()
    {
        return doorLockType;
    }

    private void SetKeyType(DoorLockType value)
    {
        doorLockType = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 7) return;
        Debug.Log(gameObject + " LayerTest Success");
        PlayerKeyManager.instance.doorLockKeys.Add(this);
        Destroy(gameObject);
    }
}
