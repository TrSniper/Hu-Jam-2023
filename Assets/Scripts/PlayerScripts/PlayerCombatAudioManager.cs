using UnityEngine;

public class PlayerCombatAudioManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource aimSource;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource getHitSource;
    [SerializeField] private AudioSource deathSource;

    public void ToggleAimSound(bool willPlay)
    {
        if (willPlay) aimSource.Play();
        else aimSource.Stop();
    }

    public void ToggleAttackSound(bool willPlay)
    {
        if (willPlay) attackSource.Play();
        else attackSource.Stop();
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
