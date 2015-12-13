using UnityEngine;
using System.Collections;
using UnityEditor;

public class CarSpawnerScript : MonoBehaviour
{

    public float spawnPeriod = 4;
    public float randomAddedPeriod = 2;
    public int difficultScalingUpTo = 10;
    public float difficultDecreaseTimeUpTo = 3.5f;

    [SerializeField]
    private GameObject carPrefab;

    private float nextSpawnTime = 0;

	// Use this for initialization
	void Start ()
	{
	    nextSpawnTime = Time.time + spawnPeriod + Random.value * randomAddedPeriod;
	}
	
	// Update is called once per frame
	void Update () {

	    if (nextSpawnTime <= Time.time)
	    {
	        Instantiate(carPrefab, transform.position, transform.rotation);

	        int difficulty = Mathf.Min(difficultScalingUpTo, WorldBuilderScript.instance.CurrentDifficulty());
	        float decrease = difficultDecreaseTimeUpTo * difficulty / difficultScalingUpTo;

            nextSpawnTime = Time.time + spawnPeriod + (Random.value * randomAddedPeriod) - decrease;
        }

	}
}
