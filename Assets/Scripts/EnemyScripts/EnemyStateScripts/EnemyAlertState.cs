using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        enemy.navMeshAgent.speed = enemy.alertSpeed;
        if (!enemy.isBrave) enemy.navMeshAgent.SetDestination(enemy.strategicPositions.positions[0]);

        enemy.eam.ToggleEnemyAim(true);
    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        base.OnUpdate(enemy);

        GoAggressiveWhenSeePlayer(enemy);
    }
}
