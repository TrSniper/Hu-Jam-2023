using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindKey : MonoBehaviour
{
    [SerializeField] BindLockType bindLockType;

    public BindLockType GetKeyType()
    {
        return bindLockType;
    }

    private void SetKeyType(BindLockType value)
    {
        bindLockType = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 7) return;
        Debug.Log(gameObject + " LayerTest Success");
        PlayerKeyManager.instance.bindLockKeys.Add(this);
        Destroy(gameObject);
    }
}

