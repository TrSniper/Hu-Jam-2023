using DG.Tweening;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    private Animator an;

    [Header("Assign")] [SerializeField] private float aimLayerWeightChangeTime = 0.2f;
    private Tweener aimLayerWeightTween;
    private float aimLayerWeight;

    private void Awake()
    {
        an = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        an.SetLayerWeight(2, aimLayerWeight);
    }

    public void ToggleEnemyAim(bool isAiming)
    {
        aimLayerWeightTween?.Kill();

        if (isAiming) aimLayerWeightTween = DOVirtual.Float(aimLayerWeight, 1, aimLayerWeightChangeTime,
            value => aimLayerWeight = value).SetEase(Ease.Linear);

        else aimLayerWeightTween = DOVirtual.Float(aimLayerWeight, 0, aimLayerWeightChangeTime,
            value => aimLayerWeight = value).SetEase(Ease.Linear);
    }
}
