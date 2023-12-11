using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyAggressiveState : EnemyBaseState
{
    private bool isAttackCooldownOver = true;

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

    private void Attack(EnemyStateManager enemy)
    {
        enemy.isAttacking = true;
        StartAttackCooldown(enemy);

        enemy.transform.LookAt(enemy.playerTransform);
        enemy.currentWeapon.Attack();

        if (!enemy.currentWeapon.isLaser && !GravityManager.isGravityActive) PlayKnockBackAnimation(-enemy.transform.forward);
        if (enemy.currentWeapon.isLaser) enemy.pcm.GetDamage(enemy.currentWeapon.damage);

        enemy.isAttacking = false;
    }

    private void Chase(EnemyStateManager enemy)
    {
        enemy.navMeshAgent.SetDestination(enemy.playerTransform.position);
    }

    private bool CanAttack(EnemyStateManager enemy)
    {
        return enemy.canSeePlayer && enemy.isPlayerInHearRange && isAttackCooldownOver;
    }

    private bool CanChase(EnemyStateManager enemy)
    {
        return !enemy.canSeePlayer || !enemy.isPlayerInHearRange;
    }

    //
    private async void StartAttackCooldown(EnemyStateManager enemy)
    {
        isAttackCooldownOver = false;
        await UniTask.WaitForSeconds(enemy.currentWeapon.cooldownTime);
        isAttackCooldownOver = true;
    }

    private void PlayKnockBackAnimation(Vector3 attackerTransformForward)
    {

    }
}
