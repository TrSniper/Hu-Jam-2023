using UnityEngine;

public class EnemyAggressiveState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        //Stop
        enemy.navMeshAgent.SetDestination(enemy.transform.position);
    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        base.OnUpdate(enemy);

        if (enemy.canSeePlayer)
        {
            Debug.Log("SEE");
        }
    }
}
