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

    }
}