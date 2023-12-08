using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour,ILock
{
    public GameObject Door;
    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject) anahtarý varsa
        GetUnlocked();
    }

    public void GetUnlocked()
    {
        DoorsOpen();
    }

    void DoorsOpen()
    { 
        //open the door
    }
}
