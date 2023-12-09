using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] List<PuzzleSolveTrigger> triggers;
    public static event Action OnAllPuzzlesComplete;

    private void Awake()
    {
        PuzzleSolveTrigger.OnPuzzleSolve += PuzzleSolveTrigger_OnPuzzleSolve;
    }

    private void PuzzleSolveTrigger_OnPuzzleSolve()
    {
        CheckForAllPuzzlesSolved();
        Debug.Log("puzzle event check");
    }

    void CheckForAllPuzzlesSolved()
    {
        float solvedPuzzlesCount = 0;
        foreach (var trigger in triggers)
        {
            if(trigger.PuzzleSolved) solvedPuzzlesCount++;
           // Debug.Log("Solved puzzles count is : " + solvedPuzzlesCount);
        }
        if(solvedPuzzlesCount == triggers.Count)
        {
         //   Debug.Log("All puzzles' are completed.");
            OnAllPuzzlesComplete?.Invoke();
        }
    }
    
}
