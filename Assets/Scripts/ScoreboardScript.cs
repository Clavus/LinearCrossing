using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardScript : SingletonComponent<ScoreboardScript>
{

    [SerializeField]
    private Text scoreboardText;

    private bool scoreboardSet = false;

    void Start()
    {
        if (!scoreboardSet)
            gameObject.SetActive(false);
    }

    public static void SetScoreBoard(int coins, CauseOfDeath cause)
    {
        //Debug.Log("Setting scoreboard to " + coins + ", " + cause);
        instance.gameObject.SetActive(true);

        string str = "";
        switch (cause)
        {
            case CauseOfDeath.CarCrash:
                str += "You were hit by a car!";
                break;
            case CauseOfDeath.FellDownHole:
                str += "You fell down a hole!";
                break;
            case CauseOfDeath.LevelReset:
                str += "Level reset!";
                break;
            case CauseOfDeath.None:
                str += "???";
                break;
        }

        str += Environment.NewLine + Environment.NewLine;
        str += "Collected " + coins + " coins!";

        instance.scoreboardText.text = str;
        instance.scoreboardSet = true;
    }

}
