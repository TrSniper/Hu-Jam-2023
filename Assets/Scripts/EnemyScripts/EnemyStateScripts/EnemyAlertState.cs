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

        if (!enemy.isBrave && enemy.navMeshAgent.remainingDistance < 0.01f)
        {
            enemy.navMeshAgent.speed = 0f;
            enemy.navMeshAgent.SetDestination(enemy.transform.position);
        }
    }
}
