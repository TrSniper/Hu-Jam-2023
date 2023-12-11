using DG.Tweening;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    private Animator an;
    private EnemyStateManager esm;

    [Header("Assign")] [SerializeField] private float aimLayerWeightChangeTime = 0.2f;
    private Tweener aimLayerWeightTween;
    private float aimLayerWeight;

    private void Awake()
    {
        an = GetComponentInChildren<Animator>();
        esm = GetComponent<EnemyStateManager>();
    }

    private void Update()
    {
        an.SetLayerWeight(2, aimLayerWeight);

        an.SetFloat("IdleOrMoving", MovingSpeedToIdleAndMoveBlendValue());
        an.SetFloat("WalkOrRun", MovingSpeedToWalkAndRunBlendValue());
    }

    public void ToggleEnemyAim(bool isAiming)
    {
        aimLayerWeightTween?.Kill();

        if (isAiming) aimLayerWeightTween = DOVirtual.Float(aimLayerWeight, 1, aimLayerWeightChangeTime,
            value => aimLayerWeight = value).SetEase(Ease.Linear);

        else aimLayerWeightTween = DOVirtual.Float(aimLayerWeight, 0, aimLayerWeightChangeTime,
            value => aimLayerWeight = value).SetEase(Ease.Linear);
    }

    private float MovingSpeedToWalkAndRunBlendValue()
    {
        return (esm.navMeshAgent.speed - esm.passiveSpeed) / (esm.alertSpeed - esm.passiveSpeed);
    }

    private float MovingSpeedToIdleAndMoveBlendValue()
    {
        return esm.navMeshAgent.speed / esm.passiveSpeed;
    }
}
