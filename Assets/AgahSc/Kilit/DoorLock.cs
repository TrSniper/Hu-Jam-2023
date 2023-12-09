using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour,ILock
{
    [Header("This component needs to be used with a parent object")]

    [SerializeField] Transform doorA_targetTransform;
    [SerializeField] Transform doorB_targetTransform;
    
    [SerializeField] GameObject doorA;
    [Tooltip("No need to use 2 doors but anyway")]
    [SerializeField] GameObject doorB;

    bool isUnlocked = false;
    bool foundKey = false;

    [SerializeField] DoorLockType type;

    public event Action OnDoorUnlocked;
    public void GetUnlocked()
    {
        Debug.Log("DoorLock Unlocked");
        isUnlocked = true;
        OnDoorUnlocked?.Invoke();
    }
    public void DoorsOpen()
    {
        if (doorA != null)
        {  
            doorA.transform.DOLocalMove(doorA_targetTransform.position, 1f);
        }
        if (doorB != null)
        {
            doorB.transform.DOLocalMove(doorB_targetTransform.position, 1f);
        }
    }

    public void DoorsClose()
    {
        if (doorA != null)
            doorA.transform.DOSmoothRewind();
        if (doorB != null)
            doorB.transform.DOSmoothRewind();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!foundKey) CheckForDoorKey();
        if(!(other.gameObject.layer == 7 || other.gameObject.layer == 11))
            return;
        if(isUnlocked)
        DoorsOpen();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!(other.gameObject.layer == 7 || other.gameObject.layer == 11))
            return;
        if (isUnlocked)
            DoorsClose();
    }

    void CheckForDoorKey()
    {
        foreach(var key in PlayerKeyManager.instance.doorLockKeys)
        {
            if(key.GetKeyType()==type)
            {
                foundKey = true;
                GetUnlocked();
            }
        }
    }
}
