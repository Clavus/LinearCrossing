using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldBuilderScript : MonoBehaviour
{

    public int cleanupDistance = 24;
    public int deactivateDistance = 12;

    [SerializeField]
    private PlayerScript player;

    [SerializeField]
    private WorldElementScript[] worldElements;

    private List<WorldElementScript> activeElements;
    private int gridDistance = 0;
    
	void Start ()
	{
	    activeElements = new List<WorldElementScript>(GetComponentsInChildren<WorldElementScript>());
        activeElements.Sort((w1, w2) => w1.transform.position.z > w2.transform.position.z ? 1
                    : (w1.transform.position.z < w2.transform.position.z ? -1 : 0));

	    foreach (WorldElementScript element in activeElements)
	    {
	        element.gridX = 0;
	        element.gridY = gridDistance;
            gridDistance += element.width;
        }
	    
        InvokeRepeating("WorldCleanup", 10, 10);
	}
	
	void Update ()
	{
	    if (player.gridY > gridDistance - 5)
	        SpawnElement();
	}

    void WorldCleanup()
    {
        if (activeElements.Count == 0)
            return;

        WorldElementScript element;
        for (int i = activeElements.Count - 1; i >= 0; i--)
        {
            element = activeElements[i];
            if (element.gridY < player.gridY - deactivateDistance)
                element.DeactivateObjects();
            if (element.gridY < player.gridY - cleanupDistance)
            {
                activeElements.RemoveAt(i);
                Destroy(element.gameObject);
            }  
        }
        
    }

    void SpawnElement()
    {
        if (worldElements.Length == 0)
            return;

        WorldElementScript element = worldElements[Random.Range(0, worldElements.Length)];
        GameObject newElement = (GameObject) Instantiate(element.gameObject, transform.position + Vector3.forward*gridDistance*Grid.Size, Quaternion.identity);
        element.gridX = 0;
        element.gridY = gridDistance;
        gridDistance += element.width;
        activeElements.Add(newElement.GetComponent<WorldElementScript>());
    }
}
