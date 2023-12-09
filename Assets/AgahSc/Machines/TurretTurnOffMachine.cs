using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurnOffMachine : MonoBehaviour
{
    [SerializeField] List<GameObject> turretsToTurnOff = new();

    [SerializeField] float turnOffTimeLimit = 5f;

    public void TurnOffTurrets() //use to turn off
    {
        foreach (var t in turretsToTurnOff)
        {
            //t.TurretOnOffSwitch();
        }
        TurnOnTurrets();
    }

    IEnumerator TurnOnTurrets() // use to turn on
    {
        yield return new WaitForSeconds(turnOffTimeLimit);
        foreach (var t in turretsToTurnOff)
        {
            //t.TurretOnOffSwitch();
        }
    }

}
