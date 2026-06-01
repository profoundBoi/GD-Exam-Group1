using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int amount);
}

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHealth = 100;

    [Header("Death")]
    public bool destroyOnDeath = true;
    public float destroyDelay = 0f;

    [Header("UI")]
    public UnityEngine.UI.Slider healthBarSlider;

    private int currentHealth;

    public bool IsDead => currentHealth <= 0;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        RefreshUI();
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0) return;
        currentHealth = Mathf.Max(0, currentHealth - amount);
        RefreshUI();
        if (IsDead) Die();
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        RefreshUI();
    }

    void Die()
    {
        if (destroyOnDeath)
            Destroy(gameObject, destroyDelay);
    }

    void RefreshUI()
    {
        if (healthBarSlider == null) return;
        healthBarSlider.value = (float)currentHealth / maxHealth;
    }
}