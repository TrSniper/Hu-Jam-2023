using Cysharp.Threading.Tasks;

public class EnemyAggressiveState : EnemyBaseState
{
    private bool isAttackCooldownOver = true;

    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);

        //Stop
        enemy.navMeshAgent.speed = 0f;
        enemy.navMeshAgent.SetDestination(enemy.transform.position);

        enemy.eam.ToggleEnemyAim(true);
    }

    public override void OnUpdate(EnemyStateManager enemy)
    {
        base.OnUpdate(enemy);

        if (CanAttack(enemy) && !enemy.isAttacking) Attack(enemy);
        else if (CanChase(enemy) && !enemy.isChasing) Chase(enemy);
    }

    private void Attack(EnemyStateManager enemy)
    {
        //Stop
        enemy.navMeshAgent.speed = 0f;
        enemy.navMeshAgent.SetDestination(enemy.transform.position);

        enemy.isAttacking = true;
        StartAttackCooldown(enemy);

        enemy.transform.LookAt(enemy.playerTransform);
        enemy.currentWeapon.Attack();

        if (!enemy.currentWeapon.isLaser && !GravityManager.isGravityActive) PlayKnockBackAnimation(enemy);
        if (enemy.currentWeapon.isLaser) enemy.pcm.GetDamage(enemy.currentWeapon.damage);

        enemy.isAttacking = false;
    }

    private void Chase(EnemyStateManager enemy)
    {
        enemy.navMeshAgent.speed = enemy.aggressiveSpeed;
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

    private void PlayKnockBackAnimation(EnemyStateManager enemy)
    {
        enemy.rb.AddForce(-enemy.transform.forward * enemy.knockBackForce);
    }
}
