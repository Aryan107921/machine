using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerBtn : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer rend;

    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private GameObject infoBox; // UI panel prefab
    
    public int defaultPrice;
    
    [SerializeField] private TextMeshProUGUI priceTxt;
    
    private int priceRecommened;


    void Start()
    {
        priceRecommened = defaultPrice;
        priceTxt.text = "Unlock Price: " + priceRecommened.ToString();

        if(EconomyManager.instance.CheckTotalIncome() >= priceRecommened)
        {
            
            rend.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {

            rend.color = new Color(1f, 1f, 1f, .6f);
            
        }
    }


    void Update()
    {
        if(EconomyManager.instance.CheckTotalIncome() >= priceRecommened)
        {
            
            rend.color = new Color(1f, 1f, 1f, 1f);
           // circleCollider2D.enabled = true;
        }
        else
        {

            rend.color = new Color(1f, 1f, 1f, .6f);
          //  circleCollider2D.enabled = false;
            
        }
    }

    public void Show()
    {
        if (infoBox != null)
        {
            infoBox.SetActive(true);
        }
    }


    public void Hide()
    {
        if (infoBox != null)
        {
            infoBox.SetActive(false);
        }
    }

}
