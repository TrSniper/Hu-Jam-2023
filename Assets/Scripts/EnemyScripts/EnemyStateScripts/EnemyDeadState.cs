using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        enemy.GetComponent<Collider>().enabled = false;
        enemy.navMeshAgent.speed = 0f;
        enemy.navMeshAgent.SetDestination(enemy.transform.position);
    }
}
