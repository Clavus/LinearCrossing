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

    [SerializeField]
    private ForbiddenSignScript forbiddenSign;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private CoinPileScript[] coinPiles;

    public int gridX = 0;
    public int gridY = 0;

    private bool died = false;
    private float scaleFactor = 1f;
    private float scaleFactorMax = 4;
    private float scaleFactorMin = 0.25f;
    private int lookDirection = 0;
    private Quaternion cageTargetRotation;
    private Vector3 targetPosition;
    private Transform cameraTransform;

    private int gameEntLayerMask;
    private int tileBlockLayerMask;

	// Use this for initialization
	void Start ()
	{
	    died = false;
	    targetPosition = transform.position;
	    cageTargetRotation = cage.rotation;
        cameraTransform = Camera.main.transform;

	    gameEntLayerMask = LayerMask.GetMask(new string[] {"GameEntity", "Geometry"});
        tileBlockLayerMask = LayerMask.GetMask(new string[] { "TileBlock" });
    }
	
	// Update is called once per frame
	void Update ()
	{

	    if (died)
	        return;

        RaycastHit hit;
	    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 24, gameEntLayerMask))
	    {
	        PickupScript pickup = hit.collider.GetComponent<PickupScript>();
	        if (pickup != null)
	        {
	            crosshair.Highlight(true);
	            crosshair.SetHitDistance(hit.distance);

	            if (Input.GetButtonDown("Grab"))
	            {
	                //Debug.Log("Picking up a " + pickup.toolType);
	                pickup.Pickup(this);
	                toolbelt.AddTool(pickup.toolType);
	            }

	        }
	        else
	        {
                crosshair.Highlight(false);
                crosshair.SetHitDistance(Mathf.Infinity);
            }
        }
	    else
	    {
            crosshair.Highlight(false);
            crosshair.SetHitDistance(Mathf.Infinity);
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

        if (Input.GetButtonDown("Use"))
        {
            OnUseTool(toolbelt.UseTool());
        }

	    if (Input.GetButtonDown("Discard"))
	    {
            toolbelt.DiscardTool();
	    }

	    cage.rotation = Quaternion.Slerp(cage.rotation, cageTargetRotation, Time.deltaTime*3f);

        // CHEATS
        if (Input.GetKeyDown(KeyCode.Keypad1))
            toolbelt.AddTool(ToolType.ForwardTranslation);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            toolbelt.AddTool(ToolType.LeftRotation);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            toolbelt.AddTool(ToolType.RightRotation);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            toolbelt.AddTool(ToolType.ScaleDown);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            toolbelt.AddTool(ToolType.ScaleUp);

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
                if (CanMoveForward())
                    MoveForward();
                else
                    forbiddenSign.Blink();
                break;
            case ToolType.ScaleUp:
                scaleFactor *= 2;
                iTween.ScaleTo(gameObject, Vector3.one * scaleFactor, 0.1f);
                if (scaleFactor > scaleFactorMax)
                {
                    CancelInvoke("PushBackSize");
                    InvokeRepeating("PushBackSize", 1f, 1f);
                }
                    
                break;
            case ToolType.ScaleDown:
                scaleFactor *= 0.5f;
                iTween.ScaleTo(gameObject, Vector3.one * scaleFactor, 0.1f);
                if (scaleFactor < scaleFactorMin)
                {
                    CancelInvoke("PushBackSize");
                    InvokeRepeating("PushBackSize", 1f, 1f);
                }
                break;
        }
    }

    void PushBackSize()
    {
        if (scaleFactor > scaleFactorMax)
            scaleFactor *= 0.5f;
        else if (scaleFactor < scaleFactorMin)
            scaleFactor *= 2;

        iTween.ScaleTo(gameObject, Vector3.one * scaleFactor, 0.1f);

        if (scaleFactor <= scaleFactorMax && scaleFactor >= scaleFactorMin)
            CancelInvoke("PushBackSize");
    }

    bool CanMoveForward()
    {
        Vector3 delta = GetMoveDelta();
        //Debug.DrawRay(transform.position, delta, Color.red, 5f);
        //Debug.DrawRay(transform.position + Vector3.up * 2f * transform.localScale.y, delta, Color.red, 5f);
        return !Physics.Raycast(transform.position, delta.normalized, delta.magnitude, tileBlockLayerMask)
            && !Physics.Raycast(transform.position + Vector3.up * 2f * transform.localScale.y, delta.normalized, delta.magnitude, tileBlockLayerMask);

        //Vector3 delta = GetMoveDelta();
        //BoxCollider box = GetComponent<BoxCollider>();
        //Vector3 center = Vector3.Scale(box.center, transform.localScale);
        //Vector3 halfSize = Vector3.Scale(box.size/2f, transform.localScale);
        //return !Physics.BoxCast(center, halfSize, delta.normalized, transform.rotation, Grid.Size, tileBlockLayerMask);
    }

    void MoveForward()
    {
        Vector3 delta = GetMoveDelta();
        targetPosition += delta;
        iTween.MoveTo(gameObject, targetPosition, 0.2f);
        IncrementGridCoord();
    }

    Vector3 GetMoveDelta()
    {
        switch (lookDirection)
        {
            case 0: return Grid.StepDelta(0, 1);
            case 1: return Grid.StepDelta(1, 0);
            case 2: return Grid.StepDelta(0, -1);
            case 3: return Grid.StepDelta(-1, 0);
        }

        return Vector3.zero;
    }

    void IncrementGridCoord()
    {
        switch(lookDirection)
        {
            case 0: gridY += 1; break;
            case 1: gridX += 1; break;
            case 2: gridY -= 1; break;
            case 3: gridX -= 1; break;
        }
    }

    public bool TooBigToFail()
    {
        return scaleFactor > 1;
    }

    public void Die()
    {
        if (died)
            return;

        died = true;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        cage.gameObject.SetActive(false);
        Fade.FadeToAlpha(1, 2f);
        Invoke("ResetLevel", 2f);
    }

    public void CollectCoins(int amount)
    {
        int toDistribute = amount;
        foreach (CoinPileScript pile in coinPiles)
        {
            if (pile.IsFull())
                continue;

            toDistribute = pile.AddCoins(toDistribute);

            if (toDistribute == 0)
                break;
        }
    }


    void ResetLevel()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

}
