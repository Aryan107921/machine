using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDragNDrop : MonoBehaviour
{
    

    public LayerMask placementLayer;     // valid ground layer
    public TowerBtn towerBtn;     // valid ground layer
    [SerializeField] private SpriteRenderer circleIndicator; // reference to circle sprite
    public LayerMask AvoidLayer;         // existing towers
    
    
    public LayerMask selfLayer;         // existing towers
    
    public float towerRadius = 0.5f;     // overlap check radius
    
    public TextMeshProUGUI messageText;     // overlap check radius
    public AudioSource selectTowerAudio;    // assign in Inspector
    public AudioSource placementTowerAudio;    // assign in Inspector

    private Camera cam;
    private GameObject previewTower;



    
    private bool isDraggin;



    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {

        if(previewTower != null && previewTower.GetComponentInChildren<Tower>())
        {
            // Follow mouse with 2D raycast
            Vector2 mousePos = cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, placementLayer);

            if (hit.collider != null)
            {
                previewTower.transform.position = hit.point;
                circleIndicator.enabled = true;
                circleIndicator.transform.position = hit.point;

                // overlap check
                bool occupied = Physics2D.OverlapCircle(hit.point, towerRadius, AvoidLayer);

                // show circle indicator
                if (circleIndicator != null)
                    circleIndicator.color = occupied ? Color.red : Color.black;
            }

            // Place tower on *second* click
            if (UnityEngine.Input.GetMouseButtonDown(0) && isDraggin)
            {
                TryPlaceTower();
            }
        }
        else
        {
            // Follow mouse with 2D raycast
            Vector2 mousePos = cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, selfLayer);

            if(hit.collider != null && hit.collider.CompareTag("TowerBtn"))
            {

                towerBtn.Show();

                if(UnityEngine.Input.GetMouseButtonDown(0) && hit.collider.CompareTag("TowerBtn"))
                {
                    if(towerBtn.defaultPrice <= EconomyManager.instance.CheckTotalIncome())
                    {
                        if (selectTowerAudio != null && EntityManager.instance.selectTowerClip != null)
                        {
                            selectTowerAudio.PlayOneShot(EntityManager.instance.selectTowerClip);
                        }
                        previewTower = Instantiate(EntityManager.instance.meleTower);
                        previewTower.GetComponentInChildren<CircleCollider2D>().enabled = false; // disable collisions while previewing
                        if(previewTower.GetComponentInChildren<Tower>()) previewTower.GetComponentInChildren<Tower>().enabled = false;
                        isDraggin = true;
                    }
                    else
                    {
                        NotEnoughMoneyMessage();
                    }
                }
            }
            else
            {
                towerBtn.Hide();
            }

        }
    }

    void NotEnoughMoneyMessage()
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
    
    void TryPlaceTower()
    {
        Vector2 pos = previewTower.transform.position;

        // Check overlap with existing towers
        bool occupied = Physics2D.OverlapCircle(pos, towerRadius, AvoidLayer);

        if (occupied)
        {
            Debug.Log("Invalid spot — already occupied!");
            ResetPreview();
            return;
        }

        if (placementTowerAudio != null && EntityManager.instance.placementTowerClip != null)
        {
            placementTowerAudio.PlayOneShot(EntityManager.instance.placementTowerClip);
        }
        // Place tower
        if(previewTower.GetComponentInChildren<Tower>()) previewTower.GetComponentInChildren<Tower>().enabled = true;
        previewTower.GetComponentInChildren<CircleCollider2D>().enabled = true;
        previewTower.layer = LayerMask.NameToLayer("Tower");
        previewTower = null;
        isDraggin = false;
        circleIndicator.enabled = false;
        EconomyManager.instance.SpendMoney(towerBtn.defaultPrice);
    }

    void ResetPreview()
    {
        if (previewTower != null)
        {
            GameObject temp = previewTower;
            previewTower = null;
            Destroy(temp);   
            
            circleIndicator.enabled = false;

            isDraggin = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerRadius);
    }

}
