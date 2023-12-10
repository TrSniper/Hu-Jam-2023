using UnityEngine;

public class EnemyAggressiveState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        //Stop
        Debug.Log("AGGRESSIVE");
        enemy.navMeshAgent.SetDestination(enemy.transform.position);
    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        base.OnUpdate(enemy);
    }
}
