using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Slider healthBar;

    private EnemyStateManager esm;

    private void Awake()
    {
        esm = GetComponent<EnemyStateManager>();
        esm.OnEnemyGetHit += UpdateHealthBar;

        //Default value
        UpdateHealthBar(10);
    }

    private void UpdateHealthBar(int newHealth)
    {
        if (newHealth <= 0) healthBar.gameObject.SetActive(false);

        healthBar.value = newHealth;
    }
}
