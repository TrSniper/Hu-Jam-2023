using System;

public class GravityManager
{
    public static event Action<bool> OnGravityChanged;

    public static bool isGravityActive { get; private set; } = true;

    public static void ActivateGravity()
    {
        isGravityActive = true;
        OnGravityChanged?.Invoke(true);
    }

    public static void DeactivateGravity()
    {
        isGravityActive = false;
        OnGravityChanged?.Invoke(false);
    }
}
