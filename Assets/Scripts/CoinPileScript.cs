using UnityEngine;
using System.Collections;

public class CoinPileScript : MonoBehaviour
{

    public int maxCoinsLower;
    public int maxCoinsUpper;

    [HideInInspector]
    public int maxCoins;

    [HideInInspector]
    public int numberOfCoins;

    [SerializeField]
    private GameObject coinPrefab;

    private int spawnedCoins;
    private Vector3 coinSize;

	// Use this for initialization
	void Start ()
	{
	    numberOfCoins = 0;
	    spawnedCoins = 0;
	    maxCoins = Random.Range(maxCoinsLower, maxCoinsUpper);
	    coinSize = coinPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsFull()
    {
        return numberOfCoins == maxCoins;
    }

    /// <summary>
    /// Adds coin to pile 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>Remaining coins if stack is full</returns>
    public int AddCoins(int amount)
    {
        int realAmount = Mathf.Min(maxCoins, numberOfCoins + amount) - numberOfCoins; // make sure we don't exceed max
        if (realAmount == 0)
            return amount;

        InvokeRepeating("SpawnCoin", 0, 0.15f);

        numberOfCoins += realAmount;
        return amount - realAmount;
    }

    void SpawnCoin()
    {
        if (spawnedCoins >= numberOfCoins)
            return;

        Vector2 rand = Random.insideUnitCircle;
        Vector3 unevenness = (Vector3.right * rand.x * coinSize.x + Vector3.forward * rand.y * coinSize.y) * 0.15f;
        GameObject coin = (GameObject)Instantiate(coinPrefab, transform.position + Vector3.up * coinSize.z * spawnedCoins + unevenness, coinPrefab.transform.rotation);
        coin.transform.parent = transform;

        spawnedCoins++;
        if (spawnedCoins >= numberOfCoins)
            CancelInvoke("SpawnCoin");
    }

}
