using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyPassiveState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Patrol(enemy);
    }

    public override void OnUpdate(EnemyStateManager enemy) { }

    private async void Patrol(EnemyStateManager enemy)
    {
        while (enemy.currentState == enemy.enemyPassiveState)
        {
            foreach (Transform node in enemy.patrolRoute.nodes)
            {
                enemy.navMeshAgent.SetDestination(node.position);
                await UniTask.WaitUntil(() => enemy.navMeshAgent.remainingDistance < 0.1f);
            }

            enemy.patrolRoute.nodes.Reverse();
        }
    }
}
