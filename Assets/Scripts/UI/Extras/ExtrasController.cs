using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class ExtrasController : MonoBehaviour
{
    public Toggle soundToggle;

    // Start is called before the first frame update
    void Start()
    {
        bool soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        if (soundEnabled)
        {
            soundToggle.isOn = true;
            AudioListener.volume = 1f;
            PlayerPrefs.SetInt("SoundEnabled", 1);
        }
        else
        {
            soundToggle.isOn = false;
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt("SoundEnabled", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetData()
    {
        DataManager.Reset();
        GameObject.FindObjectOfType<StatsManager>().ResetGame();

        GUIManager.ResetNotificationSystem();
    }

    public void ChangeSound()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1f;
            PlayerPrefs.SetInt("SoundEnabled", 1);
        }
        else
        {
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt("SoundEnabled", 0);
        }
    }
}
