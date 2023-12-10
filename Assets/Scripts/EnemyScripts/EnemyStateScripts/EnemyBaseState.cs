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
}