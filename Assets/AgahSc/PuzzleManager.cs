using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] List<PuzzleSolveTrigger> triggers;
    //[SerializeField] event Action OnPuzzleSolve; //puzzle ��z�l�nce ne mesaj� verilir de�i�ebilir ve bunu nas� implementlerim ��renmem laz�m
    IEnumerator CheckForAllPuzzlesSolved()
    {
        float solvedPuzzlesCount = 0;
        foreach (var trigger in triggers)
        {
            if(trigger.puzzleSolved) solvedPuzzlesCount++;
        }
        if(solvedPuzzlesCount == triggers.Count)
        {
            return null;
        }
        else return null;
    }
}
