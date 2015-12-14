using UnityEngine;
using System.Collections;

public class DestroyAfterSecondsScript : MonoBehaviour
{

    public float period = 1f;
    private float dieTime = 0;

	// Use this for initialization
	void Start ()
	{
	    dieTime = Time.time + period;
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (dieTime < Time.time)
            Destroy(gameObject);
	}
}
