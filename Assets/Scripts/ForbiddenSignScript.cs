using UnityEngine;
using System.Collections;

public class ForbiddenSignScript : MonoBehaviour
{

    public float blinkPeriod = 2.5f;
    public float blinkInterval = 0.5f;

    private Renderer spriteRenderer;
    private float nextBlink;
    private float stopTime;

    void Awake()
    {
        spriteRenderer = GetComponent<Renderer>();
    }
    
	void Start ()
    {
        spriteRenderer.enabled = false;
    }
	
	void Update ()
	{
	    if (stopTime <= Time.time)
	        spriteRenderer.enabled = false;
	    else
	    {
	        if (nextBlink <= Time.time)
	        {
	            spriteRenderer.enabled = !spriteRenderer.enabled;
	            nextBlink = Time.time + blinkInterval;
	        }
	    }
	}

    public void Blink()
    {
        spriteRenderer.enabled = true;
        stopTime = Time.time + blinkPeriod;
        nextBlink = Time.time + blinkInterval;
    }

}
