using UnityEngine;
using System.Collections;

public class RandomBackgroundSound : MonoBehaviour {

    public AudioClip[] backgroundSounds;

    public AudioClip backgroundRolls;
    public AudioSource audioLooper;

    float counter = 0f;
    float nextSound;

    public float minSeconds;
    public float maxSeconds;

	// Use this for initialization
	void Start () {
        counter = 0f;
        nextSound = Random.Range(minSeconds, maxSeconds);

        audioLooper.loop = true;
        audioLooper.clip = backgroundRolls;        
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if (counter >= nextSound)
        {
            counter = 0f;
            nextSound = Random.Range(minSeconds, maxSeconds);
            GetComponent<AudioSource>().PlayOneShot(backgroundSounds[(int)Mathf.Floor(Random.value * backgroundSounds.Length)]);
        }

        if (DataManager.totalMPS > 0)
        {
            if (!audioLooper.isPlaying)
                audioLooper.Play();
        }
        else
        {
            audioLooper.Stop();
        }
	}
}
