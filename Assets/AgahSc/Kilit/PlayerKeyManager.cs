using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyManager : MonoBehaviour
{
    public static PlayerKeyManager instance;
    
    public List<BindKey> bindLockKeys = new List<BindKey>();
    public List<DoorKey> doorLockKeys = new List<DoorKey>();
    private void Awake()
    {
        instance = this;
    }
}
