using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private PlayerStateData psd;
    private Animator an;

    private bool isRangedAttackAnimationPlaying;
    private bool isHeadbuttAnimationPlaying;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        an = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        //TODO: AIM and BLEND TREE
        if (psd.isAttacking) an.Play("Attack");
        else if (psd.isIdle) an.Play("Idle");
        else if (psd.isJumping) an.Play("Jump");
        else if (psd.isAscending) an.Play("Ascend");
        else if (psd.isDescending) an.Play("Descent");
        else if (psd.isWalking) an.Play("Walk");
        else if (psd.isRunning) an.Play("Run");
    }
}