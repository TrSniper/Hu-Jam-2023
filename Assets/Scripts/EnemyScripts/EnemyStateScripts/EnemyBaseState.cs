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
    }

    protected async void Patrol(EnemyStateManager enemy)
    {
        while (enemy.currentState == enemy.enemyPassiveState || enemy.currentState == enemy.enemyAlertState)
        {
            //Go forward through the list
            foreach (Vector3 node in enemy.patrolRoute.nodes)
            {
                enemy.navMeshAgent.SetDestination(node);

                await UniTask.WaitUntil(() => enemy.navMeshAgent.remainingDistance < 0.01f || enemy.currentState != enemy.enemyPassiveState ||
                                              enemy.currentState != enemy.enemyAlertState);
                
                if (enemy.currentState == enemy.enemyPassiveState || enemy.currentState == enemy.enemyAlertState) return;
            }

            //Go backward through the list
            for (int i = enemy.patrolRoute.nodes.Count - 1; i >= 0 ; i--)
            {
                enemy.navMeshAgent.SetDestination(enemy.patrolRoute.nodes[i]);

                await UniTask.WaitUntil(() => enemy.navMeshAgent.remainingDistance < 0.01f || enemy.currentState != enemy.enemyPassiveState ||
                                              enemy.currentState != enemy.enemyAlertState);

                if (enemy.currentState == enemy.enemyPassiveState || enemy.currentState == enemy.enemyAlertState) return;
            }
        }
    }

    protected void GoAggressiveWhenSeePlayer(EnemyStateManager enemy)
    {
        if (enemy.isPlayerInSightRange)
        {
            Ray ray = new Ray(enemy.transform.position, (enemy.playerTransform.position - enemy.transform.position).normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, enemy.sightWidth))
            {
                if (hit.collider.CompareTag("Player")) enemy.ChangeState(enemy.enemyAggressiveState);
            }
        }
    }
}