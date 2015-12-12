using UnityEngine;
using System.Collections;

public class CrosshairScript : MonoBehaviour
{

    private SpriteRenderer sprite;

    private Vector3 baseScale;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
