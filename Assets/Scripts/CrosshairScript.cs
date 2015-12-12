using UnityEngine;
using System.Collections;

public class CrosshairScript : MonoBehaviour
{

    private SpriteRenderer sprite;

    private Vector3 baseScale;
    private Vector3 baseDirection;
    private float baseDistance;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        baseDirection = transform.localPosition.normalized;
        baseDistance = transform.localPosition.magnitude;
        baseScale = transform.localScale;
    }
    
	void Start ()
	{

	}
	
	void Update ()
    {
	
	}

    public void SetHitDistance(float distance)
    {
        if (distance == Mathf.Infinity)
            distance = baseDistance;

        transform.localPosition = baseDirection*Mathf.Min(baseDistance, transform.InverseTransformVector(new Vector3(0,0,distance)).magnitude);
    }

    public void Highlight(bool b)
    {
        if (b)
        {
            sprite.color = Color.red;
            transform.localScale = baseScale*2;
        }
        else
        {
            sprite.color = Color.white;
            transform.localScale = baseScale;
        }
    }

}
