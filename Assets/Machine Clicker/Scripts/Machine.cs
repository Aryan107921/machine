using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement; 

public class Machine : MonoBehaviour
{

    public static Machine instance;
    [SerializeField] private int maxOxygens = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int defaultOxygens = 1;
    [SerializeField] private int defaultHealth = 30;
    [SerializeField] private Image fill;
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI leveTxt;
    [SerializeField] private TextMeshProUGUI finalStandTxt;
    [SerializeField] private TextMeshProUGUI levelMachineTxt;
    [SerializeField] private GameObject levelFailed;
    
    
    private int currentOxygens;
    private int currentHealth;

    public bool isOver;



    private int level = 1;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentOxygens = defaultOxygens;

        currentHealth = defaultHealth;
       
       
        UpdateFill();
    }


    public void IncreaseOxygens(int amount)
    {
        currentOxygens += amount;

        if(currentOxygens > maxOxygens)
        {
            NextLevel();
        }

        UpdateFill();
    }


    public void DamageHealth(int amount)
    {

        if(currentHealth > 0)
        {
            currentHealth -= amount;
            Instantiate(EntityManager.instance.machineHitFX, transform.position, transform.rotation);
        }
        else
        {
            finalStandTxt.text = "Survival Record: Level " + level.ToString();
            isOver = true;
            levelFailed.SetActive(true);
        }

        UpdateFill();
    }

    public int CheckLevel()
    {
        return level;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(1);
    } 


    private void NextLevel()
    {
        level++;
        leveTxt.text = "Level: " + level.ToString();
        levelMachineTxt.text = "Level: " + level.ToString();
        currentOxygens = defaultOxygens;
        //maxOxygens += 10;
        //defaultOxygens += 3;
        PopulationManager.instance.IncreasePopulation();

        // LeanTween animation on levelTxt
        // First reset scale to normal
        leveTxt.rectTransform.localScale = Vector3.one;

        // Animate with a punch scale (bounce effect)
        LeanTween.scale(leveTxt.rectTransform, Vector3.one * 1.3f, 0.3f)
                .setEasePunch();
    }

    private void UpdateFill()
    {
        fill.fillAmount = Mathf.InverseLerp(0, maxOxygens, currentOxygens);
        healthFill.fillAmount = Mathf.InverseLerp(0, maxHealth, currentHealth);

        if(healthFill.fillAmount < .3f) healthFill.color = Color.red;
        
        
        if(healthFill.fillAmount > .3f) healthFill.color = Color.white;
    }
}
