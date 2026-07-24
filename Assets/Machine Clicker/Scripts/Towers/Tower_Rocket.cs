using UnityEngine;

public class Tower_Rocket : Tower
{

    void Start()
    {
        UpdateTxt();
    }
    public override void Shoot(MoleCule target)
    {
        GameObject proj = Instantiate(EntityManager.instance.projectile_rocket.gameObject, firePoint.position, firePoint.rotation);
        if(proj.GetComponent<Projectile_Rocket>()) proj.GetComponent<Projectile_Rocket>().SetProjectileSND(projSpeed, projDamage);
        if(proj.GetComponent<Projectile_Rocket>()) proj.GetComponent<Projectile_Rocket>().SetTarget(target);
    }

    public override void UpdateTower()
    {
        if (updateTowerAudio != null && EntityManager.instance.updateTowerClip != null)
        {
            updateTowerAudio.PlayOneShot(EntityManager.instance.updateTowerClip);
        }
        level++;
        // Update tower fire rate
        fireRate = Mathf.Max(0.1f, fireRate - 0.2f);
        range = Mathf.Max(1.7f, range + .1f);

        // Update projectile speed on the prefab


        projDamage += 1;
    }

}
