using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyPassiveState : EnemyBaseState
{
    private bool playerAttackFlag;

    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        PlayerCombatManager.OnPlayerAttack += StupidFunction;
        enemy.navMeshAgent.speed = enemy.passiveSpeed;
        enemy.walkSource.Play();
        Patrol(enemy);
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        base.ExitState(enemy);

        PlayerCombatManager.OnPlayerAttack -= StupidFunction;
        enemy.walkSource.Stop();
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
        if (enemy.enemiesInHearRange.Length > 1) //OverlapBox and OverlapSphere counts enemy itself too
        {
            foreach (Collider detectedEnemy in enemy.enemiesInHearRange)
            {
                EnemyStateManager detectedEsm = detectedEnemy.GetComponent<EnemyStateManager>();
                if (detectedEsm.currentState == detectedEsm.enemyAlertState) enemy.ChangeState(enemy.enemyAlertState);
                else if (detectedEsm.currentState == detectedEsm.enemyAggressiveState) enemy.ChangeState(enemy.enemyAggressiveState);
            }
        }

        GoAggressiveWhenSeePlayer(enemy);

        //If hear player attack, go alert
        if (playerAttackFlag && enemy.isPlayerInHearRange) enemy.ChangeState(enemy.enemyAlertState);

        //TODO: item stolen -> alert
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
        await UniTask.Yield();
        playerAttackFlag = false;
    }
}