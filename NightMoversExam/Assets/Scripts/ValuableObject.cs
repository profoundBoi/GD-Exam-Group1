using UnityEngine;
using TMPro;
using System.Collections;
public class ValuableObject : MonoBehaviour
{
    [Header("Object Info")]
    public string objectName = "Valuable";
    public int maxValue = 100;
    [Range(0.5f, 2f)]
    public float fragilityMultiplier = 1f;
    [Header("Durability")]
    public float maxDurability = 100f;
    private float currentDurability;
    [Header("Value UI")]
    public TMP_Text valueText;
    [Header("Collision Thresholds")]
    public float grazeThreshold = 2f;
    public float bumpThreshold = 5f;
    public float impactThreshold = 10f;
    [Header("Damage Amounts")]
    public float t2Damage = 5f;
    public float t3Damage = 15f;
    public float t4Damage = 35f;
    [Header("Break Effect")]
    public GameObject breakEffect;
    private int currentValue;
    private bool isBroken = false;
    private AudioSource audioSource;
    private void Start()
    {
        currentDurability = maxDurability;
        currentValue = maxValue;
        MoneyManager.Instance.AddMoney(maxValue);
        UpdateValueUI();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;
        float velocity = collision.relativeVelocity.magnitude;
        float adjustedVelocity = velocity * fragilityMultiplier;
        DamageTier tier = GetDamageTier(adjustedVelocity);
        if (tier == DamageTier.T1_Graze)
            return;
        ApplyTierDamage(tier);
    }
    DamageTier GetDamageTier(float velocity)
    {
        if (velocity < grazeThreshold)
            return DamageTier.T1_Graze;
        if (velocity < bumpThreshold)
            return DamageTier.T2_Bump;
        if (velocity < impactThreshold)
            return DamageTier.T3_Impact;
        return DamageTier.T4_Severe;
    }
    void ApplyTierDamage(DamageTier tier)
    {
        float damage = 0f;
        switch (tier)
        {
            case DamageTier.T2_Bump:
                damage = t2Damage;
                break;
            case DamageTier.T3_Impact:
                damage = t3Damage;
                break;
            case DamageTier.T4_Severe:
                damage = t4Damage;
                break;
        }
        currentDurability -= damage;
        currentDurability = Mathf.Clamp(currentDurability, 0f, maxDurability);
        float durabilityPercent = currentDurability / maxDurability;
        int newValue = Mathf.RoundToInt(maxValue * durabilityPercent);
        int valueLost = currentValue - newValue;
        if (valueLost > 0)
        {
            MoneyManager.Instance.RemoveMoney(valueLost);
        }
        currentValue = newValue;
        UpdateValueUI();
        if (currentDurability <= 0)
        {
            StartCoroutine(BreakObjectRoutine());
        }
    }
    void UpdateValueUI()
    {
        if (valueText != null)
        {
            valueText.text = "R" + currentValue;
        }
    }
    void BreakObject()
    {

        isBroken = true;
        if (currentValue > 0)
        {
            MoneyManager.Instance.RemoveMoney(currentValue);
        }
        if (breakEffect != null)
        {
            GameObject fx = Instantiate(
                breakEffect,
                transform.position,
                Quaternion.identity
            );
            Destroy(fx, 3f);
        }
        gameObject.SetActive(false);
    }

    IEnumerator BreakObjectRoutine()
    {
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        BreakObject();
    }

    enum DamageTier
    {
        T1_Graze,
        T2_Bump,
        T3_Impact,
        T4_Severe
    }
}