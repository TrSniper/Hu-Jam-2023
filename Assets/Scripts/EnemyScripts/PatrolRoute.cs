using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [Header("Info - No Touch")]
    public List<Vector3> nodes;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            nodes.Add(transform.GetChild(i).position);
        }
    }
}