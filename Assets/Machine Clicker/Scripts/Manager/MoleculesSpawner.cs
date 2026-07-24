using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleculesSpawnerManager : MonoBehaviour
{
    
    public static MoleculesSpawnerManager instance;
    
    public GameObject tutoInfo;
    public List<Transform> molecules_0 = new List<Transform>();    
    private List<Transform> molecules_1 = new List<Transform>();    
    public Transform[] pathPoints;
    public float spawnInterval = 2f;
    
    
    private float timer;

    
    private bool isSwitch = false;
    private bool isStart = false;


    void Awake()
    {
        instance = this;
    }

    
    void Start()
    {
        StartCoroutine(StartAfterDelay(.3f));
    }

    void Update()
    {
        if(!isStart) return;

        if(Machine.instance.isOver) return;
        
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Transform randomMolecule = null;

            if(!isSwitch)
            {
                Transform temp = molecules_0[Random.Range(0, molecules_0.Count)];
                randomMolecule = temp;
                molecules_1.Add(temp);
                molecules_0.Remove(temp);

                if(molecules_0.Count <= 0) isSwitch = true;
            }
            else
            {
                Transform temp = molecules_1[Random.Range(0, molecules_1.Count)];
                randomMolecule = temp;
                molecules_0.Add(temp);
                molecules_1.Remove(temp);

                if(molecules_1.Count <= 0) isSwitch = false;
            }

            GameObject molecule = Instantiate(randomMolecule.gameObject, pathPoints[0].position, Quaternion.identity, transform);
            molecule.GetComponent<MoleCule>().pathPoints = pathPoints;
            timer = 0f;
        }
    }

    public void Start_Btn()
    {
        isStart = true;
        tutoInfo.SetActive(false);
    }

    private IEnumerator StartAfterDelay(float amount)
    {
        yield return new WaitForSeconds(amount);

        // Enable the object
        tutoInfo.SetActive(true);

        // Reset scale to zero
        tutoInfo.transform.localScale = Vector3.zero;

        // Animate to full size
        LeanTween.scale(tutoInfo, Vector3.one, 0.7f)
                 .setEaseOutBack(); 
    }
}
