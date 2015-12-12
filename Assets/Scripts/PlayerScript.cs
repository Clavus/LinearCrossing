using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    [SerializeField]
    private CrosshairScript crosshair;

    private Vector3 targetPosition;
    private Transform cameraTransform;

	// Use this for initialization
	void Start ()
	{
	    targetPosition = transform.position;
        cameraTransform = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
	    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit))
	    {
	        PickupScript pickup = hit.collider.GetComponent<PickupScript>();
	        if (pickup != null)
	        {
                crosshair.Highlight(true);

	            if (Input.GetButtonDown("Fire1"))
	            {
	                pickup.Pickup(this);
	            }

            }
	        else
                crosshair.Highlight(false);

        }
	    else
	    {
            crosshair.Highlight(false);
        }

        if (Input.GetButtonDown("Start"))
	    {
            Debug.Log("Input tracking reset");
	        InputTracking.Recenter();
	    }

        //if (Input.GetButtonDown("Back"))
        //{
        //    transform.position += new Vector3(0,0,-Grid.Size);
        //}

        if (Input.GetButtonDown("Right"))
        {
            targetPosition += new Vector3(Grid.Size, 0, 0);
            iTween.MoveTo(gameObject, targetPosition, 0.1f);
        }

        if (Input.GetButtonDown("Left"))
        {
            targetPosition += new Vector3(-Grid.Size, 0, 0);
            iTween.MoveTo(gameObject, targetPosition, 0.1f);
        }

        if (Input.GetButtonDown("Front"))
        {
            targetPosition += new Vector3(0, 0, Grid.Size);
            iTween.MoveTo(gameObject, targetPosition, 0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If other is a car
        CarScript car = other.gameObject.GetComponent<CarScript>();
        if (car != null)
        {
            car.Explode();
        }
    }

}
