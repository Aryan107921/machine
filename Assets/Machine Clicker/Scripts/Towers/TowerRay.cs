using UnityEngine;

public class TowerRay : Tower
{
    public int damage = 1;
    public LineRenderer lineRenderer; // assign in Inspector

    public AudioSource shootAudio;    // assign in Inspector

    public void Start()
    {
        UpdateTxt();

        if (lineRenderer != null)
        {
            // Basic setup
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
        }
    }

    public override void Shoot(MoleCule target)
    {
        Vector3 dir = (target.transform.position - firePoint.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, dir, out hit, range))
        {
            MoleCule m = hit.collider.GetComponent<MoleCule>();
            if (m != null)
            {
                m.TakeDamage(damage);
                Debug.Log("Ray hit " + m.name);

                // Play shoot sound
                if (shootAudio != null && EntityManager.instance.rayShootClip != null)
                {
                    shootAudio.PlayOneShot(EntityManager.instance.rayShootClip);
                }

                // Show laser beam
                if (lineRenderer != null)
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, firePoint.position);

                    // Extend the beam slightly beyond the hit point
                    float overshoot = 0.1f; // tweak this value
                    Vector3 endPos = hit.point + dir * overshoot;
                    lineRenderer.SetPosition(1, endPos);

                    // Disable after short delay
                    LeanTween.delayedCall(0.6f, () =>
                    {
                        if(lineRenderer != null) lineRenderer.enabled = false;
                    });
                }
            }

        }
    }

    public override void UpdateTower()
    {
        if (updateTowerAudio != null && EntityManager.instance.updateTowerClip != null)
        {
            updateTowerAudio.PlayOneShot(EntityManager.instance.updateTowerClip);
        }
        level++;
        damage += 1;
        // Update tower fire rate
        fireRate = Mathf.Max(0.7f, fireRate - 0.2f);
        range = Mathf.Max(1f, range + .1f);
    }
}
