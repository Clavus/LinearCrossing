using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolbeltScript : MonoBehaviour
{

    public float degreeInterval = 18;
    public float relativeScaleForSubsequentTools = 0.5f;
    [SerializeField]
    private Transform toolPositionObject;

    [SerializeField]
    private ToolTypePrefab[] toolObjectPrefabs;

    [Serializable]
    private struct ToolTypePrefab
    {
        public ToolType toolType;
        public ToolScript prefab;
    }

    private List<ToolScript> tools; 
    private Vector3 toolRelPosition;
    private Vector3 basePosition;
    private Transform cameraTransform;
    private Vector3 targetPosition;

    // Use this for initialization
    void Start ()
    {
        tools = new List<ToolScript>();
        toolRelPosition = toolPositionObject.position - transform.position;
	    basePosition = transform.position;
        targetPosition = basePosition;
        cameraTransform = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update ()
	{
	    targetPosition.x = cameraTransform.position.x;
	    targetPosition.z = cameraTransform.position.z;
	    basePosition = Vector3.Lerp(basePosition, targetPosition, Time.deltaTime * 2f);
        transform.position = basePosition + Vector3.up * 0.05f * (1 + Mathf.Sin(Time.time * 3) / 2);

	    Vector3 euler = Camera.main.transform.rotation.eulerAngles;
	    euler.x = 0;
	    euler.z = 0;
        Quaternion targetRotation = Quaternion.Euler(euler);
	    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    }

    public void AddTool(ToolType toolType)
    {
        Transform toolObjectPrefab = null;

        foreach (ToolTypePrefab toolPref in toolObjectPrefabs)
            if (toolPref.toolType == toolType)
                toolObjectPrefab = toolPref.prefab.transform;

        if (toolObjectPrefab == null)
            return;

        GameObject nt = (GameObject) Instantiate(toolObjectPrefab.gameObject, transform.position, Quaternion.identity);
        nt.transform.parent = transform;
        nt.transform.localScale = Vector3.zero;

        Vector3 movePosition = Quaternion.AngleAxis(tools.Count * -degreeInterval, Vector3.up) * (toolRelPosition / transform.localScale.x);
        iTween.MoveTo(nt, iTween.Hash("time", 0.5f, "position", movePosition, "islocal", true));

        if (tools.Count == 0)
            iTween.ScaleTo(nt, Vector3.one, 0.5f);
        else
            iTween.ScaleTo(nt, Vector3.one * relativeScaleForSubsequentTools, 0.5f);

        tools.Add(nt.GetComponent<ToolScript>());
    }

    public ToolType UseTool()
    {
        if (tools.Count == 0)
            return ToolType.None;

        ToolScript tool = tools[0];
        ToolType result = tool.GetComponent<ToolScript>().toolType;
        tools.RemoveAt(0);
        Destroy(tool.gameObject);

        Vector3 movePosition;
        for (int i = 0; i < tools.Count; i++)
        {
            movePosition = Quaternion.AngleAxis(i * -degreeInterval, Vector3.up) * (toolRelPosition / transform.localScale.x);
            iTween.MoveTo(tools[i].gameObject, iTween.Hash("time", 0.5f, "position", movePosition, "islocal", true));

            if (i == 0)
                iTween.ScaleTo(tools[i].gameObject, Vector3.one, 0.5f);
        }

        return result;

    }

}
