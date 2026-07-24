using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationManager : MonoBehaviour
{

    public static PopulationManager instance;

    [Header("UI References")]
    [SerializeField] private Image fill;
    [SerializeField] private Image coupleIcon;   // 👩‍❤️‍👨 Couple icon
    [SerializeField] private TextMeshProUGUI populationText;

    [SerializeField] private int maxPopulationCapacity = 100;
    [SerializeField] private int defaultPopulation = 10;

    [SerializeField] private int populationMultiplier = 2;
    [SerializeField] private int populationCapacityMultiplier = 2;

    private int currentPopulation = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentPopulation = defaultPopulation;
        UpdateTxt(currentPopulation);
        UpdateFill();
    }

    public void IncreasePopulation()
    {
        currentPopulation *= populationMultiplier;

        UpdateTxt(currentPopulation);
        UpdateFill();
    }

    private void UpdateFill()
    {
        fill.fillAmount = (float)currentPopulation / maxPopulationCapacity;
        
        if(fill.fillAmount < .6f) fill.color = Color.white;
        
        
        if(fill.fillAmount > .6f) fill.color = Color.red;

    }

    
    
    
    
    
    private void UpdateTxt(int _amount)
    {
        populationText.text = CurrencyFormatter.FormatCurrency(_amount);
    }


    public int CheckPopulation()
    {
        return currentPopulation;
    }
}
