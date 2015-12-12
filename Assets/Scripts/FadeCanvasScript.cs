using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeCanvasScript : MonoBehaviour
{

    public float fadeSpeed = 1;

    public static FadeCanvasScript instance;

    [SerializeField]
    private Image fadeImage;

    private Color targetColor = new Color(0,0,0,0);

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float step = Time.deltaTime*fadeSpeed*1.05f;
        Color t = fadeImage.color;
	    t.r = Mathf.MoveTowards(t.r, targetColor.r, step);
        t.g = Mathf.MoveTowards(t.g, targetColor.g, step);
        t.b = Mathf.MoveTowards(t.b, targetColor.b, step);
        t.a = Mathf.MoveTowards(t.a, targetColor.a, step);
	    fadeImage.color = t;
	}

    public void FadeToAlpha(float alpha, float fadeSeconds)
    {
        targetColor = new Color(0,0,0,alpha);
        fadeSpeed = 1/fadeSeconds;
    }

}
