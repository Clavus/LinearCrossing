using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        float scale = PlayerScript.instance.ScaleFactor;
        Vector2 rand = Random.insideUnitCircle;
        Vector3 unevenness = (Vector3.right * rand.x * coinSize.x * scale + Vector3.forward * rand.y * scale * coinSize.y) * 0.15f;
        GameObject coin = (GameObject)Instantiate(coinPrefab, transform.position + Vector3.up * coinSize.z * scale * spawnedCoins + unevenness, coinPrefab.transform.rotation);
        coin.transform.parent = transform;
        coin.transform.localScale = coinPrefab.transform.localScale;

        spawnedCoins++;

        if (spawnedCoins >= maxCoins)
            MergeMeshes();

        if (spawnedCoins >= numberOfCoins)
            CancelInvoke("SpawnCoin");
    }

    void MergeMeshes()
    {
        /*Vector3 offset = transform.position;
        foreach (Object child in transform)
            if (child is Transform)
                ((Transform) child).localPosition += offset;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length-1];
        int index = 0;
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh == null) continue;
            combine[index].mesh = meshFilters[i].sharedMesh;
            combine[index++].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].GetComponent<Renderer>().enabled = false;
            //meshFilters[i].gameObject.SetActive(false);
        }
        GetComponent<MeshFilter>().mesh = new Mesh();
        GetComponent<MeshFilter>().mesh.CombineMeshes(combine);*/

        //Debug.Log("Merged " + meshFilters.Length + " meshes for coin pile");

        Matrix4x4 myTransform = transform.worldToLocalMatrix;
        Dictionary<Material, List<CombineInstance>> combines = new Dictionary<Material, List<CombineInstance>>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            foreach (var material in meshRenderer.sharedMaterials)
                if (material != null && !combines.ContainsKey(material))
                    combines.Add(material, new List<CombineInstance>());
        }

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach (var filter in meshFilters)
        {
            if (filter.sharedMesh == null)
                continue;
            var filterRenderer = filter.GetComponent<Renderer>();
            if (filterRenderer.sharedMaterial == null)
                continue;
            if (filterRenderer.sharedMaterials.Length > 1)
                continue;
            CombineInstance ci = new CombineInstance
            {
                mesh = filter.sharedMesh,
                transform = myTransform * filter.transform.localToWorldMatrix
            };
            combines[filterRenderer.sharedMaterial].Add(ci);

            Destroy(filterRenderer);
        }

        foreach (Material m in combines.Keys)
        {
            var go = new GameObject("Combined mesh");
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh.CombineMeshes(combines[m].ToArray(), true, true);

            var arenderer = go.AddComponent<MeshRenderer>();
            arenderer.material = m;
        }
    }
}
