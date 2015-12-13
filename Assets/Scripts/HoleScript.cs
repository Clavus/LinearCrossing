using UnityEngine;
using System.Collections;

[SelectionBase]
public class HoleScript : MonoBehaviour {

    void OnTriggerStay(Collider other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null && !player.IsDead)
        {
            if (!player.TooBigToFail())
                player.Die(CauseOfDeath.FellDownHole);
        }
    }

}
