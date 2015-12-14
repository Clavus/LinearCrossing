using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolbeltScript : MonoBehaviour
{

    public float degreeInterval = 18;
    public int maxTools = 360/18; // 20
    public float relativeScaleForSubsequentTools = 0.5f;
    public Transform yawRotationTarget;

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
    private Vector3 baseLocalPosition;
    private Transform cameraTransform;

    // Use this for initialization
    void Start ()
    {
        tools = new List<ToolScript>();
        toolRelPosition = toolPositionObject.position - transform.position;
        baseLocalPosition = transform.localPosition;
        cameraTransform = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update ()
	{
	    //Vector3 camLocalPos = transform.InverseTransformVector(cameraTransform.position - transform.position);
	    //camLocalPos.y = baseLocalPosition.y;
        //baseLocalPosition = Vector3.Lerp(baseLocalPosition, camLocalPos, Time.deltaTime * 2f);
	    transform.localPosition = baseLocalPosition + Vector3.up * 0.03f * (1 + Mathf.Sin(Time.time * 2) / 2);

	    //if (yawRotationTarget != null)
	    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, yawRotationTarget.eulerAngles.y, 0), Time.deltaTime * 2f);
	}

    public bool AddTool(ToolType toolType)
    {
        Transform toolObjectPrefab = null;

        if (tools.Count >= maxTools)
            return false;

        foreach (ToolTypePrefab toolPref in toolObjectPrefabs)
            if (toolPref.toolType == toolType)
                toolObjectPrefab = toolPref.prefab.transform;

        if (toolObjectPrefab == null)
            return false;

        GameObject nt = (GameObject) Instantiate(toolObjectPrefab.gameObject, transform.position, Quaternion.identity);
        nt.transform.parent = transform;
        nt.transform.localScale = Vector3.zero;
        
        Vector3 movePosition = Quaternion.AngleAxis(tools.Count * -degreeInterval, Vector3.up) * (toolRelPosition / transform.localScale.x);
        iTween.MoveTo(nt, iTween.Hash("time", 0.5f, "position", movePosition, "islocal", true));

        nt.transform.rotation = Quaternion.LookRotation((cameraTransform.position - nt.transform.position + nt.transform.TransformVector(movePosition)).normalized, Vector3.up);

        if (tools.Count == 0)
            iTween.ScaleTo(nt, Vector3.one, 0.5f);
        else
            iTween.ScaleTo(nt, Vector3.one * relativeScaleForSubsequentTools, 0.5f);

        tools.Add(nt.GetComponent<ToolScript>());

        return true;
    }

    public ToolType UseTool()
    {
        if (tools.Count == 0)
            return ToolType.None;

        ToolScript tool = tools[0];
        ToolType result = tool.GetComponent<ToolScript>().toolType;
        tools.RemoveAt(0);
        tool.OnDiscard(true);

        UpdateToolset();

        return result;
    }

    public bool DiscardTool()
    {
        if (tools.Count == 0)
            return false;

        ToolScript tool = tools[0];
        tools.RemoveAt(0);
        tool.OnDiscard(false);

        UpdateToolset();
        return true;
    }

    void UpdateToolset()
    {

        Vector3 movePosition;
        for (int i = 0; i < tools.Count; i++)
        {
            movePosition = Quaternion.AngleAxis(i * -degreeInterval, Vector3.up) * (toolRelPosition / transform.localScale.x);
            iTween.MoveTo(tools[i].gameObject, iTween.Hash("time", 0.5f, "position", movePosition, "islocal", true));

            if (i == 0)
                iTween.ScaleTo(tools[i].gameObject, Vector3.one, 0.5f);
        }

    }

}
