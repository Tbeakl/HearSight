using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RandomiseAudio : MonoBehaviour 
{
    
    public float minDelay = 0.0f;
    public float maxDelay = 9.0f;
    
    void Start () 
    {
        
        // Just to make sure we are
        // going to disable the play
        // on awake for the AudioSource
        GetComponent<AudioSource>().playOnAwake = false;
    
        // Make sure there is an
        // AudioClip to play before
        // continuing 
        if(GetComponent<AudioSource>().clip != null)
        {
            // Get a random number between
            // our min and max delays
            float delayTime = Random.Range(minDelay, maxDelay);
            // Play the audio with a delay
            GetComponent<AudioSource>().PlayDelayed(delayTime);
        }
    }
    
}