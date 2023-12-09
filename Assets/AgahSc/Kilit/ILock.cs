public interface ILock
{
    void GetUnlocked();
}

public enum BindLockType
{
    BindLockA,
    BindLockB,
    BindLockC 
}

public enum DoorLockType
{
    DoorLockA,
    DoorLockB,
    DoorLockC 
}
