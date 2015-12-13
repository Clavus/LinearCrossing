using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinCounterScript : MonoBehaviour
{

    [SerializeField]
    private Text textField;

    private string tokenString;
    private int coins = -1;
    
	void Start ()
	{

	    tokenString = textField.text;

	}
	
	void Update ()
    {

	    if (coins != HighscoreScript.CoinsCollected)
	    {
	        coins = HighscoreScript.CoinsCollected;
	        textField.text = tokenString.Replace("%I%", coins.ToString());
	    }

	}
}
