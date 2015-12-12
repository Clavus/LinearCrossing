using System;
using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private ToolbeltScript toolbelt;

    [SerializeField]
    private CrosshairScript crosshair;

    [SerializeField]
    private Transform cage;

    private float scaleFactor = 1f;
    private int lookDirection = 0;
    private Quaternion cageTargetRotation;
    private Vector3 targetPosition;
    private Transform cameraTransform;

	// Use this for initialization
	void Start ()
	{
	    targetPosition = transform.position;
	    cageTargetRotation = cage.rotation;
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
                    toolbelt.AddTool(pickup.toolType);
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

        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("Reset"))
        {
            Fade.FadeToAlpha(1, 0.3f);
            Invoke("ResetLevel", 0.3f);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            OnUseTool(toolbelt.UseTool());
        }

	    cage.rotation = Quaternion.Slerp(cage.rotation, cageTargetRotation, Time.deltaTime*3f);

	    /*if (Input.GetButtonDown("Back"))
        {
            transform.position += new Vector3(0,0,-Grid.Size);
        }

        if (Input.GetButtonDown("Right"))
        {
            targetPosition += Grid.StepDelta(1,0);
            iTween.MoveTo(gameObject, targetPosition, 0.1f);
        }

        if (Input.GetButtonDown("Left"))
        {
            targetPosition += Grid.StepDelta(-1, 0);
            iTween.MoveTo(gameObject, targetPosition, 0.1f);
        }

        if (Input.GetButtonDown("Front"))
        {
            targetPosition += Grid.StepDelta(0, 1);
            iTween.MoveTo(gameObject, targetPosition, 0.1f);
        }*/
	}

    void OnUseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.LeftRotation:
                lookDirection -= 1;
                if (lookDirection < 0)
                    lookDirection = 3;

                cageTargetRotation = cageTargetRotation*Quaternion.AngleAxis(-90, Vector3.up);
                break;
            case ToolType.RightRotation:
                lookDirection = (lookDirection + 1) % 4;
                cageTargetRotation = cageTargetRotation * Quaternion.AngleAxis(90, Vector3.up);
                break;
            case ToolType.ForwardTranslation:
                MoveInDirection();
                break;
            case ToolType.ScaleUp:
                scaleFactor *= 2;
                iTween.ScaleTo(gameObject, Vector3.one * scaleFactor, 0.1f);
                break;
            case ToolType.ScaleDown:
                scaleFactor *= 0.5f;
                iTween.ScaleTo(gameObject, Vector3.one * scaleFactor, 0.1f);
                break;
        }
    }

    void MoveInDirection()
    {
        Vector3 delta = Vector3.zero;
        switch (lookDirection)
        {
            case 0: delta = Grid.StepDelta(0, 1); break;
            case 1: delta = Grid.StepDelta(1, 0); break;
            case 2: delta = Grid.StepDelta(0, -1); break;
            case 3: delta = Grid.StepDelta(-1, 0); break;
        }
        targetPosition += delta;
        iTween.MoveTo(gameObject, targetPosition, 0.1f);
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
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
