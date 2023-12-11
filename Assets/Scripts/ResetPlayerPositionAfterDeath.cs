using UnityEngine;

public class ResetPlayerPositionAfterDeath : MonoBehaviour
{
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;

        PlayerCombatManager.OnPlayerDeath += ResetPosition;
    }

    private void ResetPosition()
    {
        transform.position = startPosition;
    }
}
