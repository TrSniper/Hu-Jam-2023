using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        enemy.navMeshAgent.speed = enemy.alertSpeed;
        Patrol(enemy);
    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        base.OnUpdate(enemy);

        GoAggressiveWhenSeePlayer(enemy);
    }
}
