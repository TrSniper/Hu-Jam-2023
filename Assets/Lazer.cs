using UnityEngine;

public class Lazer : MonoBehaviour
{
    private PlayerCombatManager pcm;

    private void Start()
    {
        pcm = GameObject.Find("Player").GetComponent<PlayerCombatManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            pcm.GetDamage(pcm.health);
        }
    }
}
