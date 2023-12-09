using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour,ILock
{
    public GameObject Door;
    public void GetUnlocked()
    {
        DoorsOpen();
        Debug.Log("DoorLock Unlocked");
    }

    void DoorsOpen()
    { 
        //open the door
    }
}
