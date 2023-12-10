using UnityEngine;

public class EnemyAggressiveState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {

    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        Debug.Log("AGGRESSIVE");
    }
}
