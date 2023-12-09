using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeMachine : MonoBehaviour
{

    [SerializeField] List<GameObject> objectsToShake = new();

    [SerializeField] float turnOffTimeLimit = 1f;
    [SerializeField] CinemachineVirtualCamera cameraToShake;

    public void ShakeObject() 
    {
        foreach (var o in objectsToShake)
        {

        }
    }
}
