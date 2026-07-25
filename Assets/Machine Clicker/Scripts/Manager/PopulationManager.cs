using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI populationText;

    [SerializeField] private int defaultPopulation = 10;

    
    [SerializeField] private int populationIncreaser = 2;

    private int currentPopulation = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentPopulation = defaultPopulation;
        UpdateTxt(currentPopulation);
    }

    public void IncreasePopulation()
    {
        currentPopulation *= populationIncreaser;

        UpdateTxt(currentPopulation);
    }
    
    private void UpdateTxt(int _amount)
    {
        populationText.text = CurrencyFormatter.FormatCurrency(_amount);
    }

}
