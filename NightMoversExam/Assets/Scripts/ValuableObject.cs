using UnityEngine;

public class ValuableObject : MonoBehaviour
{
    [Header("Object Info")]
    public string objectName = "Valuable";
    public int maxValue = 100;

    [Range(0.5f, 2f)]
    [Tooltip("Higher = more fragile")]
    public float fragilityMultiplier = 1f;

    [Header("Durability")]
    public float maxDurability = 100f;
    private float currentDurability;

    [Header("Collision Thresholds")]
    public float grazeThreshold = 2f;   // T1
    public float bumpThreshold = 5f;    // T2
    public float impactThreshold = 10f; // T3
    // Above 10 = T4

    [Header("Damage Amounts")]
    public float t2Damage = 5f;
    public float t3Damage = 15f;
    public float t4Damage = 35f;

    [Header("Break Effect")]
    public GameObject breakEffect;

    private int currentValue;
    private bool isBroken = false;

    private void Start()
    {
        currentDurability = maxDurability;
        currentValue = maxValue;

        // Add starting value to total money
        MoneyManager.Instance.AddMoney(maxValue);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;

        float velocity = collision.relativeVelocity.magnitude;

        // Apply fragility multiplier
        float adjustedVelocity = velocity * fragilityMultiplier;

        DamageTier tier = GetDamageTier(adjustedVelocity);

        // T1 = no real damage
        if (tier == DamageTier.T1_Graze)
            return;

        ApplyTierDamage(tier);
    }

    DamageTier GetDamageTier(float velocity)
    {
        if (velocity < grazeThreshold)
        {
            return DamageTier.T1_Graze;
        }
        else if (velocity < bumpThreshold)
        {
            return DamageTier.T2_Bump;
        }
        else if (velocity < impactThreshold)
        {
            return DamageTier.T3_Impact;
        }
        else
        {
            return DamageTier.T4_Severe;
        }
    }

    void ApplyTierDamage(DamageTier tier)
    {
        float damage = 0f;

        switch (tier)
        {
            case DamageTier.T2_Bump:
                damage = t2Damage;
                Debug.Log(objectName + " suffered a BUMP.");
                break;

            case DamageTier.T3_Impact:
                damage = t3Damage;
                Debug.Log(objectName + " suffered an IMPACT.");
                break;

            case DamageTier.T4_Severe:
                damage = t4Damage;
                Debug.Log(objectName + " suffered SEVERE damage.");
                break;
        }

        currentDurability -= damage;

        currentDurability = Mathf.Clamp(currentDurability, 0f, maxDurability);

        // Recalculate object value
        float durabilityPercent = currentDurability / maxDurability;

        int newValue = Mathf.RoundToInt(maxValue * durabilityPercent);

        int valueLost = currentValue - newValue;

        if (valueLost > 0)
        {
            MoneyManager.Instance.RemoveMoney(valueLost);
        }

        currentValue = newValue;

        Debug.Log(objectName + " value is now: " + currentValue);

        // Break object
        if (currentDurability <= 0)
        {
            BreakObject();
        }
    }

    void BreakObject()
    {
        isBroken = true;

        // Remove remaining value
        if (currentValue > 0)
        {
            MoneyManager.Instance.RemoveMoney(currentValue);
        }

        Debug.Log(objectName + " BROKE!");

        // Hide renderers
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        // Spawn break effect
        if (breakEffect != null)
        {
            GameObject fx = Instantiate(
                breakEffect,
                transform.position,
                Quaternion.identity
            );

            Destroy(fx, 3f);
        }

        // Disable object after breaking
        Destroy(gameObject, 0.2f);
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    enum DamageTier
    {
        T1_Graze,
        T2_Bump,
        T3_Impact,
        T4_Severe
    }
}