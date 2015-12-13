using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrosshairScript : MonoBehaviour
{

    [SerializeField]
    private Text messageTextObject;

    private SpriteRenderer sprite;

    private Vector3 baseScale;
    private Vector3 baseDirection;
    private float baseDistance;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    
	void Start ()
	{
        baseDirection = transform.localPosition.normalized;
        baseDistance = transform.localPosition.magnitude;
        baseScale = transform.localScale;
	    RemoveMessage();
	}
	
	void Update ()
    {
	
	}

    public void SetHitDistance(float distance)
    {
        transform.localPosition = baseDirection * Mathf.Min(baseDistance, distance / PlayerScript.instance.ScaleFactor);
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

    public void ShowMessage(string str) { ShowMessage(str, Color.white); }
    public void ShowMessage(string str, Color col)
    {
        messageTextObject.text = str;
        messageTextObject.color = col;
        CancelInvoke("RemoveMessage");
        Invoke("RemoveMessage", 3f);
    }

    void RemoveMessage()
    {
        messageTextObject.text = "";
    }
}
