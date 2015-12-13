using UnityEngine;
using System.Collections;

public class HighscoreScript : MonoBehaviour
{

    public static HighscoreScript instance;

    public static int CoinsCollected
    {
        get { return instance == null ? 0 : instance.coins; }
    }

    public static int PreviousCoinsCollected
    {
        get { return instance == null ? 0 : instance.previousCoinsScore; }
    }
    
    private int coins;
    private int previousCoinsScore;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            previousCoinsScore = 0;
            coins = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        
    }

    void OnLevelWasLoaded()
    {
        previousCoinsScore = coins;
        coins = 0;
    }

    public static void AddCoins(int amount)
    {
        if (instance == null)
            return;

        instance.coins += amount;
    }

}
