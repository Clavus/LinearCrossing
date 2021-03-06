﻿using System;
using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public bool startScreenMode = false;

    [SerializeField]
    private ToolbeltScript toolbelt;

    [SerializeField]
    private CrosshairScript crosshair;

    [Header("Cage configuration")]
    [SerializeField]
    private Transform cage;

    [SerializeField]
    private ParticleSystem cageParticleSystem;

    [SerializeField]
    private Transform cageEnginePiston;

    [SerializeField]
    private ForbiddenSignScript forbiddenSign;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private CoinPileScript[] coinPiles;

    [Header("Movement raycasts")]
    [SerializeField]
    private Transform rayCastPivot;

    [SerializeField]
    private Transform[] moveRayTransforms;

    [SerializeField]
    private Transform resizeRayTransform;

    [Header("Audio")]
    [SerializeField]
    private AudioSource carCrashAudio;

    [SerializeField]
    private AudioSource coinCollectAudio;

    [SerializeField]
    private AudioSource grabAudio;

    [SerializeField]
    private AudioSource useAudio;

    [SerializeField]
    private AudioSource deniedAudio;

    [SerializeField]
    private AudioSource discardAudio;

    [SerializeField]
    private AudioSource fallingAudio;

    public static PlayerScript instance;

    public float ScaleFactor { get { return scaleFactor; } }
    public float GrabRange { get { return grabRange; } }
    public bool IsDead { get { return died; } }
    public int GridX { get { return gridX; } }
    public int GridY { get { return gridY; } }

    private int gridX = 0;
    private int gridY = 0;
    private int travelledGridSquares = 0;

    private bool died = false;
    private float grabRange;
    private float scaleFactor = 1f;
    private float scaleFactorMax = 2;
    private float scaleFactorMin = 0.25f;
    private int lookDirection = 0;
    private float nextInteractTime = 0;
    private Quaternion cageTargetRotation;
    private Vector3 targetPosition;
    private Transform cameraTransform;
    private Rigidbody body;

    private int gameEntLayerMask;
    private int tileBlockLayerMask;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        body = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start ()
	{
	    died = false;
	    targetPosition = transform.position;
	    cageTargetRotation = cage.rotation;
        cameraTransform = Camera.main.transform;

	    gameEntLayerMask = LayerMask.GetMask(new string[] {"GameEntity", "Geometry"});
        tileBlockLayerMask = LayerMask.GetMask(new string[] { "TileBlock" });

        UpdateGrabRange();
    }
	
	// Update is called once per frame
	void Update ()
	{

	    if (died)
	        return;

        RaycastHit hit;
	    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, GrabRange, gameEntLayerMask))
	    {
	        IInteractable interactable = hit.collider.GetComponent(typeof (IInteractable)) as IInteractable;
	        if (interactable != null)
	        {
	            crosshair.Highlight(true);
	            crosshair.SetHitDistance(hit.distance);

	            if (Input.GetButtonDown("Grab") && nextInteractTime < Time.time)
	            {
	                //Debug.Log("Picking up a " + pickup.toolType);
	                nextInteractTime = Time.time + 0.1f;
                    interactable.OnInteract(this);
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
	        InputTracking.Recenter();

	        if (startScreenMode)
	        {
                FadeCanvasScript.instance.FadeToAlpha(1, 2);
                Invoke("LoadTutorial", 2f);
            }
                
	    }

        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("Reset") && !startScreenMode)
        {
            Die(CauseOfDeath.LevelReset);
        }

        if (Input.GetButtonDown("Use") && !startScreenMode)
        {
            OnUseTool(toolbelt.UseTool());
        }

	    if (Input.GetButtonDown("Discard"))
	    {
            if (toolbelt.DiscardTool())
                if (discardAudio != null)
                    discardAudio.Play();
        }

	    cage.rotation = Quaternion.Slerp(cage.rotation, cageTargetRotation, Time.deltaTime*3f);

        // CHEATS
	    if (Application.isEditor && !startScreenMode)
	    {
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
        }
    }

    public void ShowCrosshairMessage(string str, Color color)
    {
        crosshair.ShowMessage(str, color);
    }
    
    public bool TryAddTool(ToolType toolType)
    {
        if (toolbelt.AddTool(toolType))
        {
            if (grabAudio != null)
                grabAudio.Play();

            return true;
        } 
        else
        {
            if (deniedAudio != null)
                deniedAudio.Play();

            crosshair.ShowMessage("Max reached!", Color.red);
            return false;
        }

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
                rayCastPivot.rotation = cageTargetRotation;
                if (useAudio != null)
                    useAudio.Play();

                break;
            case ToolType.RightRotation:
                lookDirection = (lookDirection + 1)%4;
                cageTargetRotation = cageTargetRotation*Quaternion.AngleAxis(90, Vector3.up);
                rayCastPivot.rotation = cageTargetRotation;
                if (useAudio != null)
                    useAudio.Play();

                break;
            case ToolType.ForwardTranslation:
                if (CanMoveForward())
                {
                    MoveForward();
                    if (useAudio != null)
                        useAudio.Play();
                }
                else
                {
                    forbiddenSign.Blink();
                    crosshair.ShowMessage("Obstructed!", Color.red);
                    if (deniedAudio != null)
                        deniedAudio.Play();
                }  
                break;
            case ToolType.ScaleUp:
                if (CanGrow())
                {
                    Grow();
                    if (useAudio != null)
                        useAudio.Play();
                }
                else
                {
                    crosshair.ShowMessage("Obstructed!", Color.red);
                    if (deniedAudio != null)
                        deniedAudio.Play();
                }
                    
                break;
            case ToolType.ScaleDown:
                Shrink();
                if (useAudio != null)
                    useAudio.Play();

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

    public void Shrink()
    {
        if (scaleFactor < scaleFactorMin)
            return;

        scaleFactor *= 0.5f;
        iTween.ScaleTo(gameObject, Vector3.one * scaleFactor, 0.1f);
        UpdateGrabRange();
        if (scaleFactor < scaleFactorMin)
        {
            CancelInvoke("PushBackSize");
            InvokeRepeating("PushBackSize", 1f, 1f);
        }
    }

    public void Grow()
    {
        if (scaleFactor > scaleFactorMax)
            return;

        scaleFactor *= 2;
        iTween.ScaleTo(gameObject, Vector3.one * scaleFactor, 0.1f);
        UpdateGrabRange();
        if (scaleFactor > scaleFactorMax)
        {
            CancelInvoke("PushBackSize");
            InvokeRepeating("PushBackSize", 1f, 1f);
        }
    }

    bool CanMoveForward()
    {
        bool result = true;
        foreach (Transform rayOrigin in moveRayTransforms)
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.forward * Grid.Size, Color.red, 3);
            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, Grid.Size, tileBlockLayerMask))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    bool CanGrow()
    {
        Vector3 ray = (resizeRayTransform.position - transform.position);
        Debug.DrawRay(resizeRayTransform.position, ray, Color.red, 3);
        return !Physics.Raycast(resizeRayTransform.position, ray.normalized, ray.magnitude, tileBlockLayerMask);
    }

    void MoveForward()
    {
        Vector3 delta = GetMoveDelta();
        targetPosition += delta;
        iTween.MoveTo(gameObject, targetPosition, 0.2f);
        IncrementGridCoord();

        travelledGridSquares++;
        HighscoreScript.SetDistanceTravelled((int) (travelledGridSquares * Grid.Size));

        cageParticleSystem.Play();
        iTween.Stop(cageEnginePiston.gameObject);
        Invoke("PunchPiston", 0); // do a frame later
    }

    void PunchPiston()
    {
        iTween.PunchPosition(cageEnginePiston.gameObject, Vector3.up * 0.2f * scaleFactor, 1f);
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

    public void Die(CauseOfDeath cause)
    {
        if (died)
            return;

        died = true;

        switch (cause)
        {
            case CauseOfDeath.CarCrash:
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                cage.gameObject.SetActive(false);
                if (carCrashAudio != null)
                    carCrashAudio.Play();

                Fade.FadeToAlpha(1, 0.3f);
                Invoke("ResetLevel", 2f);
                break;
            case CauseOfDeath.FellDownHole:
                GetComponent<Collider>().enabled = false;
                body.useGravity = true;
                body.isKinematic = false;
                body.velocity = Vector3.zero;
                Fade.FadeToAlpha(1, 0.5f);
                Invoke("PlayFallAudio", 0.1f);
                Invoke("ResetLevel", 2f);
                break;
            case CauseOfDeath.LevelReset:
                Fade.FadeToAlpha(1, 0.3f);
                Invoke("ResetLevel", 0.3f);
                break;
            default:
                Fade.FadeToAlpha(1, 0.3f);
                Invoke("ResetLevel", 0.3f);
                break;
        }
        
        HighscoreScript.SetCauseOfDeath(cause);
    }

    void PlayFallAudio()
    {
        if (fallingAudio != null)
            fallingAudio.Play();
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

        coinCollectAudio.Play();
        HighscoreScript.AddCoins(amount);
    }

    void UpdateGrabRange()
    {
        grabRange = Grid.Size * 9 * Mathf.Sqrt(scaleFactor);
    }

    void LoadTutorial()
    {
        SceneManager.LoadScene("tutorial", LoadSceneMode.Single);
    }

    void ResetLevel()
    {
        SceneManager.LoadScene("game", LoadSceneMode.Single);
    }

}

public enum CauseOfDeath
{
    CarCrash, FellDownHole, LevelReset, None
}

