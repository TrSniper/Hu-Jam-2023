using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolveTrigger : MonoBehaviour
{
    public static event Action OnPuzzleSolve;
    private enum TriggerType
    {
        PlayerTrigger,
        PuzzleObjectTrigger
    }
    [SerializeField] TriggerType type;

    [Tooltip("Don't touch.")]
    public bool PuzzleSolved { get; private set; } = false;
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (!(other.gameObject.layer == 7 || other.gameObject.layer == 10)) return;

        if (type == TriggerType.PlayerTrigger && other.gameObject.layer == 7)
        {
            //player trigger
            PuzzleSolved = true;
            //Debug.Log("player puzzle Solved");
            OnPuzzleSolve?.Invoke();
        }

        if(type == TriggerType.PuzzleObjectTrigger && other.gameObject.layer == 10)
        {
            //puzzle trigger
            PuzzleSolved = true;
            //Debug.Log("object puzzle Solved");
            OnPuzzleSolve?.Invoke();
        }

    }
}