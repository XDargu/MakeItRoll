using UnityEngine;
using System.Collections;

public class RollAudioStart : MonoBehaviour {

    public AudioClip[] startClips;
    public float fadeOffset = 0.5f;
    public float fadeTime = 1f;
    float offsetCounter;
    float counter;

    bool canPlay = true;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        offsetCounter = Mathf.Max(offsetCounter - Time.deltaTime, 0);
        if (offsetCounter <= 0)
        {
            if (counter > 0)
            {
                counter -= Time.deltaTime;
                GetComponent<AudioSource>().volume = counter / fadeTime;
                if (counter <= fadeTime * 0.6f)
                {
                    canPlay = true;
                }
            }
        }
	}

    public void StartClip()
    {
        if (canPlay)
        {
            //canPlay = false;
            GetComponent<AudioSource>().clip = startClips[(int)Mathf.Floor(Random.value * startClips.Length)];
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().volume = 1f;
            counter = fadeTime;
            offsetCounter = fadeOffset;
        }
    }
}
