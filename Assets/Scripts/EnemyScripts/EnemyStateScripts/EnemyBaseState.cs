using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyBaseState
{
    public virtual void EnterState(EnemyStateManager enemy) { }
    public virtual void ExitState(EnemyStateManager enemy) { }

    public virtual void OnUpdate(EnemyStateManager enemy)
    {
        enemy.sightArea = new Vector3(enemy.sightWidth, enemy.sightHeight, enemy.sightWidth);
        enemy.sightAreaCenter = enemy.transform.position + enemy.transform.forward * ((enemy.sightWidth - enemy.transform.lossyScale.z) / 2);

        enemy.isPlayerInHearRange = Physics.CheckSphere(enemy.transform.position, enemy.hearRange, enemy.playerLayer);
        enemy.isPlayerInSightRange = Physics.CheckBox(enemy.sightAreaCenter, enemy.sightArea / 2 ,Quaternion.identity, enemy.playerLayer);

        //canSeePlayer
        if (enemy.isPlayerInSightRange)
        {
            Vector3 rayPosition = enemy.transform.position;
            rayPosition.y += 1;

            Ray ray = new Ray(rayPosition, (enemy.playerTransform.position - enemy.transform.position).normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, enemy.sightWidth)) enemy.canSeePlayer = hit.collider.CompareTag("Player");
        }
    }

    protected async void Patrol(EnemyStateManager enemy)
    {
        while (CanPatrol(enemy))
        {
            //Go forward through the list
            foreach (Vector3 node in enemy.patrolRoute.nodes)
            {
                enemy.navMeshAgent.SetDestination(node);

                await UniTask.WaitUntil(() => enemy.navMeshAgent.remainingDistance < 0.01f || !CanPatrol(enemy));
                if (!CanPatrol(enemy)) return;
            }

            //Go backward through the list
            for (int i = enemy.patrolRoute.nodes.Count - 1; i >= 0 ; i--)
            {
                enemy.navMeshAgent.SetDestination(enemy.patrolRoute.nodes[i]);

                await UniTask.WaitUntil(() => enemy.navMeshAgent.remainingDistance < 0.01f || !CanPatrol(enemy));
                if (!CanPatrol(enemy)) return;
            }
        }
    }

    private bool CanPatrol(EnemyStateManager enemy)
    {
        return enemy.currentState == enemy.enemyPassiveState || (enemy.isBrave && enemy.currentState == enemy.enemyAlertState);
    }

    protected void GoAggressiveWhenSeePlayer(EnemyStateManager enemy)
    {
        if (enemy.canSeePlayer) enemy.ChangeState(enemy.enemyAggressiveState);
    }
}