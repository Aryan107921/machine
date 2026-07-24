using UnityEngine.UI;
using UnityEngine;
using Unity.Mathematics;





public enum InputType
{
    H20,
    Bug,
}




public class MoleCule : MonoBehaviour
{
    public int DamageAmount = 10;
    public int rewardAmount = 10;
    public int IncreaseAmount = 1;
    public float speed = 7f;
    public int maxHealth = 6;
    public int healthIncremeantAmount = 1;
    public int defaultHealth = 6;

    public Transform[] pathPoints;
    public Image healthBarFill;
    public Transform healthBar;
    private int currentPoint = 0;

    public InputType type;

    private int currentHealth;

    void Start()
    {
        maxHealth = currentHealth = defaultHealth + (healthIncremeantAmount * Machine.instance.CheckLevel());
        if(type != InputType.H20) UPdateHealthBarFill();
    }

    void Update()
    {
        if(Machine.instance.isOver) return;
        if (pathPoints.Length == 0) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            pathPoints[currentPoint].position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, pathPoints[currentPoint].position) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= pathPoints.Length)
            {
                if(type == InputType.H20)
                {
                    Machine.instance.IncreaseOxygens(IncreaseAmount);
                }
                else 
                {
                    Machine.instance.DamageHealth(DamageAmount);
                }

                Destroy(gameObject); // end  
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= amount;
            UPdateHealthBarFill();
        }
        else
        {
            EconomyManager.instance.Reward(rewardAmount);
            Instantiate(EntityManager.instance.bugDestroyFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void UPdateHealthBarFill()
    {
        healthBar.gameObject.SetActive(true);
        healthBarFill.fillAmount = Mathf.InverseLerp(0, maxHealth, currentHealth);
    }
}
