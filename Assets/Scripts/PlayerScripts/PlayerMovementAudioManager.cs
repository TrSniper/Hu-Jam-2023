using UnityEngine;

public class PlayerMovementAudioManager : MonoBehaviour
{
    [Header("Assign - Gravity")]
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource runSource;
    [SerializeField] private AudioSource jumpSource;

    [Header("Assign - Zero Gravity")]
    [SerializeField] private AudioSource zeroGravityMovementSource;
    [SerializeField] private AudioSource ascendSource;
    [SerializeField] private AudioSource descentSource;

    private bool isWalkSourcePlaying;
    private bool isRunSourcePlaying;
    private bool isJumpSourcePlaying;

    private bool isZeroGravityMovementPlaying;
    private bool isAscendSourcePlaying;
    private bool isDescentSourcePlayingPlaying;

    private PlayerStateData psd;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
    }

    private void Update()
    {
        if (GravityManager.isGravityActive)
        {
            //Walk - Run Group
            if (!psd.isMoving)
            {
                isWalkSourcePlaying = false;
                walkSource.Stop();

                isRunSourcePlaying = false;
                //runSource.Stop();
            }
            else if (psd.isWalking && !isWalkSourcePlaying)
            {
                isWalkSourcePlaying = true;
                walkSource.Play();

                isRunSourcePlaying = false;
                //runSource.Stop();
            }
            else if (psd.isRunning && !isRunSourcePlaying)
            {
                isWalkSourcePlaying = false;
                walkSource.Stop();

                isRunSourcePlaying = true;
                //runSource.Play();
            }

            //Jump Group
            if (psd.isJumping && !isJumpSourcePlaying)
            {
                isJumpSourcePlaying = true;
                jumpSource.Play();
            }
            else if (!psd.isJumping && isJumpSourcePlaying)
            {
                isJumpSourcePlaying = false;
                jumpSource.Stop();
            }
        }

        else
        {
            //Movement Group
            if (psd.isMoving && !isZeroGravityMovementPlaying)
            {
                zeroGravityMovementSource.Play();
                isZeroGravityMovementPlaying = true;
            }
            else if (!psd.isMoving && isZeroGravityMovementPlaying)
            {
                zeroGravityMovementSource.Stop();
                isZeroGravityMovementPlaying = false;
            }

            //Ascend Group
            if (psd.isAscending && !isAscendSourcePlaying)
            {
                ascendSource.Play();
                isAscendSourcePlaying = true;
            }
            else if (!psd.isAscending && isAscendSourcePlaying)
            {
                ascendSource.Stop();
                isAscendSourcePlaying = false;
            }

            //Descend Group
            if (psd.isDescending && !isDescentSourcePlayingPlaying)
            {
                descentSource.Play();
                isDescentSourcePlayingPlaying = true;
            }
            else if (!psd.isDescending && isDescentSourcePlayingPlaying)
            {
                descentSource.Stop();
                isDescentSourcePlayingPlaying = false;
            }
        }
    }
}
