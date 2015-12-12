using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour
{

    public ToolType toolType;

    private Vector3 basePosition;
    private Quaternion targetRotation;
    private Transform cameraTransform;

	// Use this for initialization
	void Start ()
	{

	    basePosition = transform.position;
	    cameraTransform = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update ()
	{

	    transform.position = basePosition + Vector3.up*0.1f*(1+Mathf.Sin(Time.time*3)/2);

        Quaternion look = Quaternion.LookRotation((cameraTransform.position - transform.position).normalized, Vector3.up);
	    targetRotation = Quaternion.Euler(0, look.eulerAngles.y, 0);
	    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);

	}

    public void Pickup(PlayerScript player)
    {
        iTween.MoveTo(gameObject, Camera.main.transform.position + Vector3.down, 1f);
        iTween.ScaleTo(gameObject, Vector3.zero, 1f);
        Invoke("DestroyMe", 1f);
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}

