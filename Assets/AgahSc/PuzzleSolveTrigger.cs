using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolveTrigger : MonoBehaviour
{
    private enum TriggerType
    {
        PlayerTrigger,
        PuzzleObjectTrigger
    }
    [SerializeField] TriggerType type;
    public bool puzzleSolved = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == 7 || other.gameObject.layer == 10)) return;

        if (type == TriggerType.PlayerTrigger && other.gameObject.layer == 7)
        {
            //player trigger
            puzzleSolved = true;
        }

        if(type == TriggerType.PuzzleObjectTrigger && other.gameObject.layer == 10)
        {
            //puzzle trigger
            puzzleSolved = true;
        }
        
    }
}
