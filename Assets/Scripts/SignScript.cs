using UnityEngine;
using System.Collections;

public class SignScript : MonoBehaviour
{

    public float activationDelay = 0;
    public float activationDistance = 3;

    private bool visible = false;
    private Transform cameraTransform;
    private Vector3 baseScale;

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

	void Start ()
	{
        visible = false;
	    canvas.enabled = false;
	    baseScale = transform.localScale;
	    transform.localScale = Vector3.one*0.05f;
	    cameraTransform = Camera.main.transform;
	}
	
	void Update ()
	{
	    if (!visible && Vector3.Distance(cameraTransform.position, transform.position) <= activationDistance)
	    {
	        Invoke("ShowSign", activationDelay);
            visible = true;
        }
	}

    void ShowSign()
    {
        canvas.enabled = true;
        iTween.ScaleTo(gameObject, iTween.Hash(new object[] { "scale", baseScale, "time", 2f, "easetype", iTween.EaseType.easeOutElastic }));
    }
}
