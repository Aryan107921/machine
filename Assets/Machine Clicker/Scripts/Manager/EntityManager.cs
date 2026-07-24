using UnityEngine;

public class EntityManager : MonoBehaviour
{
    

    public static EntityManager instance;

    public Transform projectile_mele;
    public Transform projectile_rocket;

    public GameObject meleTower;
    public GameObject  rocketTower;
    public GameObject  laserTower;
    
    public GameObject  bugDestroyFX;
    public GameObject  machineHitFX;

    public AudioClip rayShootClip;       // assign in Inspector
    public AudioClip selectTowerClip;       // assign in Inspector
    public AudioClip placementTowerClip;       // assign in Inspector
    public AudioClip updateTowerClip;       // assign in Inspector

    void Awake()
    {
        instance = this;
    }
}
