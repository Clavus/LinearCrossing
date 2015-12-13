using UnityEngine;
using System.Collections;

public class HoleScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            if (!player.TooBigToFail())
                player.Die(CauseOfDeath.FellDownHole);
        }
    }

}
