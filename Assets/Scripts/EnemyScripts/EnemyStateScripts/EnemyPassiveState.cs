using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyPassiveState : EnemyBaseState
{
    private bool playerAttackFlag;

    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        Patrol(enemy);

        PlayerCombatManager.OnPlayerAttack += StupidFunction;
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        base.ExitState(enemy);

        PlayerCombatManager.OnPlayerAttack -= StupidFunction;
    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        base.OnUpdate(enemy);

        enemy.enemiesInSightRange = Physics.OverlapBox(enemy.sightAreaCenter, enemy.sightArea / 2, Quaternion.identity, enemy.enemyLayer);
        enemy.enemiesInHearRange = Physics.OverlapSphere(enemy.transform.position, enemy.hearRange, enemy.enemyLayer);

        //If see dead enemy, go alert
        if (enemy.enemiesInSightRange.Length > 1) //OverlapBox and OverlapSphere counts enemy itself too
        {
            foreach (Collider e in enemy.enemiesInSightRange)
            {
                EnemyStateManager esm = e.GetComponent<EnemyStateManager>();
                if (esm.currentState == esm.enemyDeadState) enemy.ChangeState(enemy.enemyAlertState);
            }
        }

        //If hear alert enemy go alert, if hear aggressive enemy go aggressive
        if (enemy.enemiesInHearRange.Length > 0)
        {
            foreach (Collider detectedEnemy in enemy.enemiesInHearRange)
            {
                EnemyStateManager detectedEsm = detectedEnemy.GetComponent<EnemyStateManager>();
                if (detectedEsm.currentState == detectedEsm.enemyAlertState) enemy.ChangeState(enemy.enemyAlertState);
                else if (detectedEsm.currentState == detectedEsm.enemyAggressiveState) enemy.ChangeState(enemy.enemyAggressiveState);
            }
        }

        //If see player, go aggressive
        if (enemy.isPlayerInSightRange)
        {
            Ray ray = new Ray(enemy.transform.position, (enemy.playerTransform.position - enemy.transform.position ).normalized);
            enemy.canSeePlayer = Physics.Raycast(ray, enemy.sightWidth, enemy.playerLayer);
            if (enemy.canSeePlayer) enemy.ChangeState(enemy.enemyAggressiveState);
        }

        //If hear player attack, go alert
        if (playerAttackFlag && enemy.isPlayerInHearRange) enemy.ChangeState(enemy.enemyAlertState);

        //TODO: item stolen -> alert
    }

    private async void Patrol(EnemyStateManager enemy)
    {
        //Reason why we have separate while loops is enemy can encounter with player anytime during the patrol loop

        while (enemy.currentState == enemy.enemyPassiveState)
        {
            //Go forward through the list
            foreach (Vector3 node in enemy.patrolRoute.nodes)
            {
                enemy.navMeshAgent.SetDestination(node);
                await UniTask.WaitUntil(() => enemy.navMeshAgent.remainingDistance < 0.1f || enemy.currentState != enemy.enemyPassiveState);
            }
        }

        while (enemy.currentState == enemy.enemyPassiveState)
        {
            //Go backward through the list
            for (int i = enemy.patrolRoute.nodes.Count - 1; i >= 0 ; i--)
            {
                enemy.navMeshAgent.SetDestination(enemy.patrolRoute.nodes[i]);
                await UniTask.WaitUntil(() => enemy.navMeshAgent.remainingDistance < 0.1f || enemy.currentState != enemy.enemyPassiveState);
            }
        }
    }

    //What we actually needed to do is:
    // if (playerAttackFlag && enemy.isPlayerInHearRange) enemy.ChangeState(enemy.enemyAlertState);
    //inside of the function, subscribe it in the EnterState() and unsubscribe it in the ExitState()

    //But since this function is subscribed to an "event Action", only the "event invoker" can send parameter, or we must use anonymous..
    //..method. We can't use anonymous method because we can't unsubscribe it. So the solution I found is making a flag true for one frame to..
    //..catch it in OnUpdate()

    private async void StupidFunction()
    {
        playerAttackFlag = true;
        await UniTask.Yield();
        playerAttackFlag = false;
    }
}