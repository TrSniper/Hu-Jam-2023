using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Awake()
    {
        PlayerCombatManager.OnHealthChanged += UpdateHealthBar;

        //Default value
        UpdateHealthBar(10);
    }

    private void UpdateHealthBar(int newHealth)
    {
        healthBar.value = newHealth;
        healthText.text = $"Health: {newHealth}";
    }
}
