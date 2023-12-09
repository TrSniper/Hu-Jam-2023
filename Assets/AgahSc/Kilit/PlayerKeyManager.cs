using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyManager : MonoBehaviour
{
    public bool HasKey { get; private set; }

    private List<Key> keys = new List<Key>();

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == 8))
            return;
        keys.Add(other.gameObject.GetComponent<Key>());
        HasKey = true;
        Debug.Log("player has a key now");
        Debug.Log(HasKey);
    }
    //private void Update()
    //{
    //    if (!(keys.Count >= 1))
    //        return;
    //    HasKey = true;
    //}
}
