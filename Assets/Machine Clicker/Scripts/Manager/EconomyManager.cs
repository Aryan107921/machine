using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    
    public static EconomyManager instance;

    
    [SerializeField] private Image fill;

    [SerializeField] private TextMeshProUGUI economyText;
    [SerializeField] private int maxTotalIncomeCapacity = 1000;
    
    
    [SerializeField] private int defaultTotalIncome = 20;
    
    
    [SerializeField] private int perCapita = 2;

    private int CurrentTotalIncome = 0;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CurrentTotalIncome = defaultTotalIncome;
        UpdateFill();
        UpdateTxt(CurrentTotalIncome);
    }

    public void Reward(int amount)
    {
        // First once
        CurrentTotalIncome += amount;

        UpdateFill();
        UpdateTxt(CurrentTotalIncome);
    }

    private void UpdateFill()
    {
        fill.fillAmount = (float)CurrentTotalIncome / maxTotalIncomeCapacity;

        
        if(fill.fillAmount < .6f) fill.color = Color.white;
        
        
        if(fill.fillAmount > .6f) fill.color = Color.red;
    }


    public void SpendMoney(int amount)
    {
        if(CurrentTotalIncome >= amount)
        {
            CurrentTotalIncome -= amount;
            UpdateFill();
            UpdateTxt(CurrentTotalIncome);
        }
        else
        {
            return;
        }
    }

    public int CheckTotalIncome()
    {
        return CurrentTotalIncome;
    }
    private void UpdateTxt(int _amount)
    {
        economyText.text = CurrencyFormatter.FormatCurrency(_amount);
    }

}
