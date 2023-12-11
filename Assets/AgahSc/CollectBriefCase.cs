using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBriefCase : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            player.GetComponent<PlayerWinCondition>().canWin = true;
            Destroy(gameObject);
        }
    }
}
