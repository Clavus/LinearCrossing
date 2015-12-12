using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldElementScript : MonoBehaviour
{

    public int width;

    [HideInInspector]
    public bool objectsActive = true;
    [HideInInspector]
    public int gridX;
    [HideInInspector]
    public int gridY;

    private List<GameObject> objects;

    void Start()
    {
        objects = new List<GameObject>();
        
        foreach(var com in GetComponentsInChildren<SpawnerScript>())
            objects.Add(com.gameObject);
        foreach (var com in GetComponentsInChildren<CarSpawnerScript>())
            objects.Add(com.gameObject);
    }

    public void DeactivateObjects()
    {
        if (!objectsActive)
            return;

        objectsActive = false;
        foreach(GameObject obj in objects)
            obj.SetActive(false);
    }

    public void ReactivateObjects()
    {
        if (objectsActive)
            return;

        objectsActive = true;
        foreach (GameObject obj in objects)
            obj.SetActive(true);
    }

}
