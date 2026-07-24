using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using System.Collections;

public class Tower : MonoBehaviour
{
    
    public float projSpeed = 10f;
    
    public int projDamage = 1;
    public float fireRate = 1f;
    public float hideRate = 1f;
    public float range = 4f;
    
    public Transform firePoint; // where projectile spawns
    
    
    public LayerMask selfLayer; 
    
    
    protected float fireTimer;
    protected float hideTimer;

    
    
    public int updatePriceRecommened = 100; // money 
    public int priceMultiplier = 2;  
    
    public GameObject info; // UI panel prefab
    public TextMeshProUGUI leveltxt;      // text inside the panel

    public TextMeshProUGUI priceTxt; // assign in Inspector
    public TextMeshProUGUI messageText; // assign in Inspector
    public AudioSource updateTowerAudio; // assign in Inspector
    protected int level = 1;


    void Start()
    {
        UpdateTxt();
    }

    void Update()
    {
        if(Machine.instance.isOver) return;


        Vector2 mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        Collider2D m_hit = Physics2D.OverlapPoint(mousePos, selfLayer);

        if (m_hit != null && m_hit.CompareTag("Tower"))
        {
            m_hit.GetComponent<Tower>().ShowInfo();
        }

        fireTimer += Time.deltaTime;
        Debug.Log(fireTimer);
        if (fireTimer >= fireRate)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, range);
            MoleCule target = null;
            float minDist = Mathf.Infinity;

            foreach (Collider hit in hits)
            {
                MoleCule m = hit.GetComponent<MoleCule>();
                if (m != null && m.type != InputType.H20)
                {
                    float dist = Vector3.Distance(transform.position, m.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = m;
                    }
                }
            }

            if (target != null)
            {
                RotateToward(target.transform.position);
                Shoot(target);
                fireTimer = 0f;
            }
        }
    }

    public void RotateToward(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

    }



    public void TryUpdate(int price)
    {
        if(price >= updatePriceRecommened)
        {
            UpdateTower();
            EconomyManager.instance.SpendMoney(updatePriceRecommened);
            updatePriceRecommened *= priceMultiplier;
            UpdateTxt();
            UPdatedMessage();
        }
        else
        {
            NotEnoughMoneyMessage();
        }
    }






    public void UPdatedMessage()
    {
        // Show message
        messageText.text = "Updated!";
        messageText.color = Color.red;

        // Animate with LeanTween (shake effect)
        LeanTween.scale(messageText.rectTransform, Vector3.one * 1.2f, 0.2f)
                    .setEasePunch();

        // Optionally fade back to normal
        LeanTween.delayedCall(1f, () =>
        {
            messageText.text = "";
            messageText.color = Color.white;
        });
    }

    public void NotEnoughMoneyMessage()
    {
        // Show message
            messageText.text = "Not enough money!";
            messageText.color = Color.red;

            // Animate with LeanTween (shake effect)
            LeanTween.scale(messageText.rectTransform, Vector3.one * 1.2f, 0.2f)
                     .setEasePunch();

            // Optionally fade back to normal
            LeanTween.delayedCall(1f, () =>
            {
                messageText.text = "";
                messageText.color = Color.white;
            });
    }

    
    




    public void OnMouseDown()
    {
        TryUpdate(EconomyManager.instance.CheckTotalIncome());
    }


     // Hover events
    public void ShowInfo()
    {
        if (info != null)
        {
            info.SetActive(true);
            StartCoroutine(HideAfterDelay(2f));
        }
    }

    
    
    
    
    
    
    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        info.SetActive(false);
    }

    public void UpdateTxt()
    {
        leveltxt.text = "Level: " + level;
        priceTxt.text = "Next upgrade: " + updatePriceRecommened.ToString();
    }

    public virtual void Shoot(MoleCule target)
    {
        GameObject proj = Instantiate(EntityManager.instance.projectile_mele.gameObject, firePoint.position, firePoint.rotation);
        if(proj.GetComponent<Projectile>()) proj.GetComponent<Projectile>().SetProjectileSND(projSpeed, projDamage);
        if(proj.GetComponent<Projectile>()) proj.GetComponent<Projectile>().SetTarget(target);
    }
    public void OnDrawGizmosSelected()
    {
        // Draw a wireframe sphere in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    
    
    public virtual void UpdateTower()
    {
        if (updateTowerAudio != null && EntityManager.instance.updateTowerClip != null)
        {
            updateTowerAudio.PlayOneShot(EntityManager.instance.updateTowerClip);
        }

        level++;
        // Update tower fire rate
        fireRate = Mathf.Max(0.7f, fireRate - 0.2f);
        range = Mathf.Max(1.7f, range + .1f);
    
        projDamage += 1;
    }
}
