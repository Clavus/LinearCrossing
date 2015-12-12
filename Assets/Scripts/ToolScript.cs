using UnityEngine;
using System.Collections;

public class ToolScript : MonoBehaviour
{

    public ToolType toolType;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Quaternion look = Quaternion.LookRotation((cameraTransform.position - transform.position).normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime*0.5f);
    }

}
