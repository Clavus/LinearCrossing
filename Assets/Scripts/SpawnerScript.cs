using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnerScript : MonoBehaviour
{

    [Range(0,1f)]
    public float chanceToExist = 1;

    public float spawnDelay = 8;

    [SerializeField]
    private GameObject[] spawnableObjects;

    private GameObject spawned;
    private float spawnNextTime;
    private bool spawnedActive = false;

	// Use this for initialization
	void Start ()
	{
	    if (Random.value > chanceToExist)
	    {
            Destroy(gameObject);
	        return;
	    }

	    Renderer r = GetComponent<Renderer>();
	    if (r != null)
	        r.enabled = false;

	}

    void OnEnable()
    {
        if (spawned != null)
            spawned.SetActive(true);
    }

    void OnDisable()
    {
        if (spawned != null)
            spawned.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
	{

	    if (spawnableObjects.Length == 0)
	        return;

        if (!spawnedActive && spawnNextTime <= Time.time)
            SpawnObject();

	    if (spawnedActive && spawned == null && spawnNextTime <= Time.time)
	    {
	        spawnedActive = false;
	        spawnNextTime = Time.time + spawnDelay;
	    }
	}

    void SpawnObject()
    {
        spawnedActive = true;

        GameObject prefab = spawnableObjects[Random.Range(0, spawnableObjects.Length)];
        if (prefab != null)
        {
            spawned = (GameObject)Instantiate(prefab, transform.position, prefab.transform.rotation);
            spawned.transform.parent = transform;
        }
            
    }

}
