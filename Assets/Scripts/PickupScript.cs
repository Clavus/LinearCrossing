using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour
{

    public ToolType toolType;

    private Vector3 basePosition;

	// Use this for initialization
	void Start ()
	{

	    basePosition = transform.position;

	}
	
	// Update is called once per frame
	void Update ()
	{

	    transform.position = basePosition + Vector3.up*0.1f*(1+Mathf.Sin(Time.time*3)/2);

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

