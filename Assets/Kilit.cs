using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kilit : MonoBehaviour
{
    //bu class böyle göt oldu sabah bakýcam.
}
public enum BindLockType
{
    BindLockA,
    BindLockB,
    BindLockC //may not use
}

public enum DoorLockType
{
   DoorLockA,
   DoorLockB,
   DoorLockC //may not use
}
public enum MachineLockType
{
    MachineLockA,
    MachineLockB,
    MachineLockC //may not use
}

public interface ILock
{
    void GetUnlocked();
}
