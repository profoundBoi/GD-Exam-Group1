using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    void TakeDamage(int amount);
}

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Events")]
    public UnityEvent<int, int> onHealthChanged; 
    public UnityEvent onDeath;
    public UnityEvent<int> onDamaged;          
    public UnityEvent<int> onHealed;           

    [Header("Optional UI")]
    [Tooltip("Assign a UI Slider to visualise health automatically.")]
    public UnityEngine.UI.Slider healthBarSlider;

    [Header("Death")]
    [Tooltip("Destroy the GameObject on death? Disable if you handle it yourself.")]
    public bool destroyOnDeath = true;
    public float destroyDelay = 0f;

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
        onDamaged?.Invoke(amount);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        RefreshUI();

        if (IsDead)
            Die();
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealed?.Invoke(amount);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        RefreshUI();
    }

    public void SetMaxHealth(int newMax, bool refillToMax = false)
    {
        maxHealth = Mathf.Max(1, newMax);
        if (refillToMax)
            currentHealth = maxHealth;
        else
            currentHealth = Mathf.Min(currentHealth, maxHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);
        RefreshUI();
    }

    public void InstantKill() => TakeDamage(currentHealth);

    void Die()
    {
        onDeath?.Invoke();

        if (destroyOnDeath)
            Destroy(gameObject, destroyDelay);
    }

    void RefreshUI()
    {
        if (healthBarSlider == null) return;
        healthBarSlider.value = maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
    }
}