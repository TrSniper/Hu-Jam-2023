using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {

    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        Debug.Log("ALERT");
    }
}
