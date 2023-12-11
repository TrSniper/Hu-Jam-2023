using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCheck : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            if (player.GetComponent<PlayerWinCondition>().canWin)
                SceneManager.LoadScene(2); // 2 You Win sahnesi olmalý.
    }
}

