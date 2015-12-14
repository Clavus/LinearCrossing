using UnityEngine;
using System.Collections;

public class HonkScript : MonoBehaviour
{

    public AudioSource sound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null && sound != null)
        {
            sound.Play();
        }
    }
}
