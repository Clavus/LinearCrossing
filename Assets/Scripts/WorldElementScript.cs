using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldElementScript : MonoBehaviour
{

    public int Width { get { return width; } }
    public int Difficulty { get { return difficulty; } }

    [SerializeField]
    private int width;
    [SerializeField]
    private int difficulty = 0;

    [HideInInspector]
    public bool objectsActive;
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

        objectsActive = true;
        DeactivateObjects();
    }

    public void DeactivateObjects()
    {
        if (!objectsActive)
            return;

        objectsActive = false;
        foreach(GameObject obj in objects)
            if (obj != null)
                obj.SetActive(false);
    }

    public void ActivateObjects()
    {
        if (objectsActive)
            return;

        objectsActive = true;
        foreach (GameObject obj in objects)
            if (obj != null)
                obj.SetActive(true);
    }

}
