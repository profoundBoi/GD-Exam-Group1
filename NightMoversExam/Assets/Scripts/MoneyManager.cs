using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [Header("Money")]
    public int totalMoney = 0;

    [Header("Sliders")]
    public GameObject Player1Slider, Player2Slider;
    public UnityEngine.UI.Slider player1MoneySlider, player2MoneySlider;

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
        player1MoneySlider = Player1Slider.GetComponent<UnityEngine.UI.Slider>();
        player2MoneySlider = Player2Slider.GetComponent<UnityEngine.UI.Slider>();
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

    private void Update()
    {
        if (player1MoneySlider != null && player2MoneySlider != null)
        {
            player1MoneySlider.value = totalMoney;
            player2MoneySlider.value = totalMoney;
        }
    }
}