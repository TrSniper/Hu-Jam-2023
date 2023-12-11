using UnityEngine;

public class PlayerCombatAudioManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource aimSource;
    [SerializeField] private AudioSource laserAttackSource;
    [SerializeField] private AudioSource projectileAttackSource;
    [SerializeField] private AudioSource getHitSource;
    [SerializeField] private AudioSource deathSource;

    public void ToggleAimSound(bool willPlay)
    {
        if (willPlay) aimSource.Play();
        else aimSource.Stop();
    }

    public void ToggleLaserAttackSound(bool willPlay)
    {
        if (willPlay) laserAttackSource.Play();
        else laserAttackSource.Stop();
    }

    public void ToggleProjectileSound(bool willPlay)
    {
        if (willPlay) projectileAttackSource.Play();
        else projectileAttackSource.Stop();
    }

    public void ToggleGetHitSound(bool willPlay)
    {
        if (willPlay) getHitSource.Play();
        else getHitSource.Stop();
    }

    public void ToggleDeathSound(bool willPlay)
    {
        if (willPlay) deathSource.Play();
        else deathSource.Stop();
    }
}
