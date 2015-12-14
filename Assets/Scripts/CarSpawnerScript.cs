using UnityEngine;
using System.Collections;

public class CarSpawnerScript : MonoBehaviour
{

    public float spawnPeriod = 4;
    public float randomAddedPeriod = 2;
    public int difficultScalingUpTo = 10;
    public float difficultDecreaseTimeUpTo = 3.5f;

    [SerializeField]
    private GameObject[] carPrefabs;

    private float nextSpawnTime = 0;

	// Use this for initialization
	void Start ()
	{
	    nextSpawnTime = Time.time + spawnPeriod + Random.value * randomAddedPeriod;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (carPrefabs.Length == 0)
	        return;

	    if (nextSpawnTime <= Time.time)
	    {
	        Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], transform.position, transform.rotation);

	        int difficulty = Mathf.Min(difficultScalingUpTo, WorldBuilderScript.instance.CurrentDifficulty());
	        float decrease = difficultDecreaseTimeUpTo * difficulty / difficultScalingUpTo;

            nextSpawnTime = Time.time + spawnPeriod + (Random.value * randomAddedPeriod) - decrease;
        }

	}
}
