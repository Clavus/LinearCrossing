using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeCanvasScript : SingletonComponent<FadeCanvasScript>
{

    public float fadeSpeed = 1;
    
    [SerializeField]
    private Image fadeImage;

    private Color targetColor = new Color(0,0,0,0);

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
        Color t = fadeImage.color;
	    if (targetColor != t)
	    {
            float step = Time.deltaTime * fadeSpeed * 1.05f;
            t.r = Mathf.MoveTowards(t.r, targetColor.r, step);
            t.g = Mathf.MoveTowards(t.g, targetColor.g, step);
            t.b = Mathf.MoveTowards(t.b, targetColor.b, step);
            t.a = Mathf.MoveTowards(t.a, targetColor.a, step);
        }
	    fadeImage.color = t;
	}

    public void FadeToAlpha(float alpha, float fadeSeconds)
    {
        targetColor = new Color(0,0,0,alpha);
        fadeSpeed = 1/fadeSeconds;
    }

}
