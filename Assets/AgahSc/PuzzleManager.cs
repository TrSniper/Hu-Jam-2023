using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] List<PuzzleSolveTrigger> triggers;
    //[SerializeField] event Action OnPuzzleSolve; //puzzle çözülünce ne mesajý verilir deðiþebilir ve bunu nasý implementlerim öðrenmem lazým
    void CheckForAllPuzzlesSolved()
    {
        float solvedPuzzlesCount = 0;
        foreach (var trigger in triggers)
        {
            if(trigger.puzzleSolved) solvedPuzzlesCount++;
        }
        if(solvedPuzzlesCount == triggers.Count)
        {
            //OnAllPuzzlesComplete?.Invoke();
        }
    }
}
