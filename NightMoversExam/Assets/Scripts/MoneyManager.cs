using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [Header("Money")]
    public int totalMoney = 0;

    [Header("UI")]
    public TMP_Text moneyText;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateMoneyUI();
    }

    // Adds money to total
    public void AddMoney(int amount)
    {
        totalMoney += amount;

        if (totalMoney < 0)
            totalMoney = 0;

        UpdateMoneyUI();

        Debug.Log("Money Added: +" + amount);
        Debug.Log("Current Total: " + totalMoney);
    }

    // Removes money from total
    public void RemoveMoney(int amount)
    {
        totalMoney -= amount;

        if (totalMoney < 0)
            totalMoney = 0;

        UpdateMoneyUI();

        Debug.Log("Money Lost: -" + amount);
        Debug.Log("Current Total: " + totalMoney);
    }

    // Updates UI text
    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Total Value: R" + totalMoney;
        }
    }

    // Optional getter
    public int GetMoney()
    {
        return totalMoney;
    }
}