using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour, IInteractable
{

    public ToolType toolType;

    private bool visible = true;
    private Vector3 basePosition;
    private Vector3 baseScale;
    private Quaternion targetRotation;
    private Transform cameraTransform;

	// Use this for initialization
	void Start ()
	{

	    basePosition = transform.position;
	    baseScale = transform.localScale;
	    cameraTransform = Camera.main.transform;

        transform.localScale = Vector3.zero;
        visible = false;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (visible)
	    {
	        transform.position = basePosition + Vector3.up*0.1f*(1 + Mathf.Sin(Time.time*3)/2);

	        Quaternion look = Quaternion.LookRotation((cameraTransform.position - transform.position).normalized,
	            Vector3.up);
	        targetRotation = Quaternion.Euler(0, look.eulerAngles.y, 0);
	        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*0.5f);
	    }

	    if (visible && Vector3.Distance(cameraTransform.position, transform.position) >= PlayerScript.instance.GrabRange)
	        Shrink();
        else if (!visible && Vector3.Distance(cameraTransform.position, transform.position) <= PlayerScript.instance.GrabRange)
            Grow();

	}

    void Shrink()
    {
        visible = false;
        iTween.ScaleTo(gameObject, Vector3.zero, 0.5f);
    }

    void Grow()
    {
        visible = true;
        iTween.ScaleTo(gameObject, iTween.Hash(new object[] { "scale", baseScale, "time", 4f, "easetype", iTween.EaseType.easeOutElastic } ));
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void OnInteract(PlayerScript player)
    {
        if (player.TryAddTool(toolType))
        {
            GetComponent<Collider>().enabled = false;
            iTween.MoveTo(gameObject, Camera.main.transform.position + Vector3.down, 1f);
            iTween.ScaleTo(gameObject, Vector3.zero, 1f);
            Invoke("DestroyMe", 1f);
        }
    }
}

