﻿using UnityEngine;
using System.Collections;

public class HighscoreScript : SingletonComponent<HighscoreScript>
{
    public static int CoinsCollected
    {
        get { return instance == null ? 0 : instance.coins; }
    }

    private int coins;
    private int travelledMeters;
    private CauseOfDeath previousDeathCause = CauseOfDeath.None;

    void Start()
    {
        if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded()
    {
        if (coins > 0 || previousDeathCause != CauseOfDeath.None)
            ScoreboardScript.SetScoreBoard(coins, travelledMeters, previousDeathCause);

        previousDeathCause = CauseOfDeath.None;
        coins = 0;
    }

    public static void SetCauseOfDeath(CauseOfDeath cause)
    {
        if (instance == null)
            return;

        instance.previousDeathCause = cause;
    }

    public static void SetDistanceTravelled(int meters)
    {
        if (instance == null)
            return;

        instance.travelledMeters = meters;
    }

    public static void AddCoins(int amount)
    {
        if (instance == null)
            return;

        instance.coins += amount;
    }

}
