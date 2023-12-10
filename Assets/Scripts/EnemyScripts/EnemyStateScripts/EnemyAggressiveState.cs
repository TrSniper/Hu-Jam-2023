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

        if (CanAttack(enemy) && !enemy.isAttacking) Attack(enemy);
        else if (CanChase(enemy) && !enemy.isChasing) Chase(enemy);
    }

    private async void Attack(EnemyStateManager enemy)
    {

    }

    private async void Chase(EnemyStateManager enemy)
    {

    }

    private bool CanAttack(EnemyStateManager enemy)
    {
        return enemy.canSeePlayer;
    }

    private bool CanChase(EnemyStateManager enemy)
    {
        return !enemy.canSeePlayer;
    }

    private void BreakAttack(EnemyStateManager enemy)
    {

    }

    private void BreakChase(EnemyStateManager enemy)
    {

    }
}
