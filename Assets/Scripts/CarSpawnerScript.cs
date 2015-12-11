using UnityEngine;
using System.Collections;
using UnityEditor;

public class CarSpawnerScript : MonoBehaviour
{

    public float spawnPeriod = 3;

    [SerializeField]
    private GameObject carPrefab;

    private float nextSpawnTime = 0;

	// Use this for initialization
	void Start ()
	{
	    nextSpawnTime = Time.time + spawnPeriod;
	}
	
	// Update is called once per frame
	void Update () {

	    if (nextSpawnTime <= Time.time)
	    {
	        Instantiate(carPrefab, transform.position, transform.rotation);
            nextSpawnTime = Time.time + spawnPeriod;
        }



	}
}
