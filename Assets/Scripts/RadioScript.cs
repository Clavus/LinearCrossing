using UnityEngine;
using System.Collections;

public class RadioScript : SingletonComponent<RadioScript>, IInteractable
{

    private AudioSource[] sources;

    private static int current = -1;
    private static float trackTime = 0;
    private static AudioClip nowPlayingClip;
    
    private Color messageColor = new Color(0,255,45);
    private AudioSource nowPlaying;
    private float nextTrackTime = 0;

	// Use this for initialization
	void Start () {

        sources = GetComponents<AudioSource>();

        if (instance == this && current != -1 && sources[current].clip == nowPlayingClip)
	    {
            sources[current].Play();
            sources[current].time = trackTime;
	        nextTrackTime = Time.unscaledTime + nowPlayingClip.length - trackTime;
            nowPlaying = sources[current];
	    }
	    else
	    {
	        current = -1;
	    }

	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (current == -1)
	        return;

	    trackTime = nowPlaying.time;

        if (nextTrackTime < Time.unscaledTime)
            SwitchToNextTrack();

	}

    public void OnInteract(PlayerScript player)
    {
        SwitchToNextTrack(true);
        
        if (current == -1)
            player.ShowCrosshairMessage("Turned off radio", messageColor);
        else
            player.ShowCrosshairMessage("Playing track " + (current + 1) + " of " + sources.Length, messageColor);
    }

    public void SwitchToNextTrack(bool stopAfterLast = false)
    {
        if (current != -1)
            sources[current].Stop();

        if (sources.Length == 0)
            return;

        current++;
        if (!stopAfterLast)
            current = current % sources.Length;

        if (current < sources.Length)
        {
            nextTrackTime = Time.unscaledTime + sources[current].clip.length + 1f;
            sources[current].Play();
            nowPlaying = sources[current];
            nowPlayingClip = nowPlaying.clip;
        }
        else if (stopAfterLast)
            current = -1;
        
    }
}
