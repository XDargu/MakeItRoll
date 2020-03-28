using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayServicesManager : MonoBehaviour
{
    private bool mWaitingForAuth = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting Google Play Services");
        GooglePlayGames.PlayGamesPlatform.Activate();
        if (!Social.localUser.authenticated)
        {
            // Authenticate
            mWaitingForAuth = true;
            Social.localUser.Authenticate((bool success) =>
            {
                mWaitingForAuth = false;
                if (success)
                {
                    AchievementManager.playServicesLoaded = true;
                    Debug.Log("Welcome " + Social.localUser.userName);
                }
                else
                {
                    Debug.Log("Authentication failed.");
                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
