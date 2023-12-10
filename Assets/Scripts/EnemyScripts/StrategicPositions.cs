using UnityEngine;

public class StrategicPositions : MonoBehaviour
{
    [Header("Info - No Touch")] public Vector3[] positions;

    private void Awake()
    {
        positions = new Vector3[2];

        positions[0] = transform.GetChild(0).position;
        positions[1] = transform.GetChild(1).position;
    }
}