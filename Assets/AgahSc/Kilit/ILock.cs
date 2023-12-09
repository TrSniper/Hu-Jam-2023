public interface ILock
{
    void GetUnlocked();
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
