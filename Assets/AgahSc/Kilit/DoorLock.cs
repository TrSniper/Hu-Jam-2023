using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour, ILock
{
    [SerializeField] Transform doorA_targetTransform;
    [SerializeField] Transform doorB_targetTransform;
    Vector3 doorA_startPos;
    Vector3 doorB_startPos;

    [SerializeField] GameObject doorA;
    [Tooltip("No need to use 2 doors but anyway")]
    [SerializeField] GameObject doorB;

    bool isUnlocked = false;
    bool foundKey = false;

    [SerializeField] DoorLockType type;

    public event Action OnDoorUnlocked;
    private void Start()
    {
        if(doorA_startPos != null)
        doorA_startPos = doorA.transform.position;
        if (doorA_startPos != null)
            doorB_startPos = doorB.transform.position;

    }
    private void OnGUI()
    {
      bool open =  GUI.Button(new Rect(Screen.width / 2 +256, Screen.height / 2, 128, 64), "Open Doors");
        bool close = GUI.Button(new Rect(Screen.width / 2 + 256, Screen.height / 2 + 128, 128, 64), "Close Doors");

        if (open) DoorsOpen();
        if (close) DoorsClose();
    }
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
            doorA.transform.DOLocalMove(doorA_targetTransform.position, 3f);
        }
        if (doorB != null)
        {
            doorB.transform.DOLocalMove(doorB_targetTransform.position, 3f);
        }
    }

    public void DoorsClose()
    {
        if (doorA != null)
            doorA.transform.DOLocalMove(doorA_startPos, 3f);
        if (doorB != null)
            doorB.transform.DOLocalMove(doorB_startPos, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!(other.gameObject.layer == 7 || other.gameObject.layer == 11))
            return;
        if (!foundKey) CheckForDoorKey();
        if (isUnlocked)
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
        foreach (var key in PlayerKeyManager.instance.doorLockKeys)
        {
            if (key.GetKeyType() == type)
            {
                foundKey = true;
                GetUnlocked();
            }
        }
    }
}
