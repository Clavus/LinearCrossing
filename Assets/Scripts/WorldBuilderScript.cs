﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldBuilderScript : SingletonComponent<WorldBuilderScript>
{

    public int tilesPerDifficultyIncrease = 6;
    [Header("Number of Y grid squares ahead")]
    public int creationDistance = 30;
    public int activateDistance = 8;
    [Header("Number of Y grid squares behind")]
    public int cleanupDistance = 12;
    public int deactivateDistance = 8;

    [SerializeField]
    private PlayerScript player;

    [SerializeField]
    private WorldElementScript[] worldElements;

    private WorldElementScript lastChosenElement;
    private WorldElementScript secondLastChosenElement;

    private List<WorldElementScript> activeElements;
    private int gridDistance = 0;

	void Start ()
	{
        if (instance != this)
            Destroy(gameObject);

        activeElements = new List<WorldElementScript>(GetComponentsInChildren<WorldElementScript>());
        activeElements.Sort((w1, w2) => w1.transform.position.z > w2.transform.position.z ? 1
                    : (w1.transform.position.z < w2.transform.position.z ? -1 : 0));

	    foreach (WorldElementScript element in activeElements)
	    {
	        element.gridX = 0;
	        element.gridY = gridDistance;
            gridDistance += element.Width;
            element.ActivateObjects();
        }

        InvokeRepeating("WorldActivator", 3, 3);
        InvokeRepeating("WorldCleanup", 10, 10);
	}
	
	void Update ()
	{
	    if (player.GridY > gridDistance - creationDistance)
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
            if (element.gridY < player.GridY - deactivateDistance)
                element.DeactivateObjects();
            if (element.gridY < player.GridY - cleanupDistance)
            {
                activeElements.RemoveAt(i);
                Destroy(element.gameObject);
            }  
        }
        
    }

    private void WorldActivator()
    {
        foreach (WorldElementScript element in activeElements)
        {
            if (!element.objectsActive && element.gridY > player.GridY &&
                element.gridY < player.GridY + activateDistance)
            {
                //Debug.Log("activating " + element.gameObject.name + ", gridY: " + element.gridY + ", player gridY: " + player.gridY);
                element.ActivateObjects();
            }
        }
    }

    void SpawnElement()
    {
        if (worldElements.Length == 0)
            return;

        int spawnDifficulty = gridDistance/tilesPerDifficultyIncrease;
        List<WorldElementScript> potentialElements = new List<WorldElementScript>();
        foreach(WorldElementScript el in worldElements)
            if (el.Difficulty <= spawnDifficulty)
                potentialElements.Add(el);

        if (potentialElements.Count == 0)
            return;

        // make sure we don't spawn the same element 3 times in a row
        WorldElementScript chosenElement = null;
        int attempts = 10;
        while (attempts > 0 && (chosenElement == null || (chosenElement == lastChosenElement && chosenElement == secondLastChosenElement)))
        {
            chosenElement = potentialElements[Random.Range(0, potentialElements.Count)];
            attempts--;
        }

        secondLastChosenElement = lastChosenElement;
        lastChosenElement = chosenElement;

        //Debug.Log("Spawning " + chosenElement.name + " (" + chosenElement.Difficulty + ") adding " + chosenElement.Width + " to grid distance " + gridDistance + " with difficulty " + spawnDifficulty);

        GameObject newElement = (GameObject) Instantiate(chosenElement.gameObject, transform.position + Vector3.forward*gridDistance*Grid.Size, chosenElement.transform.rotation);
        WorldElementScript nElement = newElement.GetComponent<WorldElementScript>();
        nElement.gridX = 0;
        nElement.gridY = gridDistance;
        nElement.transform.parent = transform;

        gridDistance += chosenElement.Width;
        activeElements.Add(nElement);
    }

    public int CurrentDifficulty()
    {
        return player.GridY / tilesPerDifficultyIncrease;
    }
}
