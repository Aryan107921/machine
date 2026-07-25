using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    
    public static EconomyManager instance;


    [SerializeField] private TextMeshProUGUI economyText;
    
    
    [SerializeField] private int defaultTotalIncome = 20;
    

    private int CurrentTotalIncome = 0;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CurrentTotalIncome = defaultTotalIncome;
        UpdateTxt(CurrentTotalIncome);
    }

    public void Reward(int amount)
    {
        // First once
        CurrentTotalIncome += amount;
        UpdateTxt(CurrentTotalIncome);
    }


    public void SpendMoney(int amount)
    {
        if(CurrentTotalIncome >= amount)
        {
            CurrentTotalIncome -= amount;
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
