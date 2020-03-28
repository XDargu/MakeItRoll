using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class AchievementManager : MonoBehaviour {

    public static bool playServicesLoaded = false;

    float counter = 0f;
    public float updateRate = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Logros
        if (playServicesLoaded)
        {
            counter += Time.deltaTime;
            if (counter >= updateRate)
            {
                counter = 0f;
                if (DataManager.meters >= 1)
                {
                    Social.ReportProgress("CgkI5cGT4tUGEAIQBQ", 100.0f, (bool success) =>
                    {
                        // handle success or failure
                    });
                }

                if (DataManager.meters >= 100)
                {
                    Social.ReportProgress("CgkI5cGT4tUGEAIQBg", 100.0f, (bool success) =>
                    {
                        // handle success or failure
                    });
                }

                if (DataManager.meters >= 1000000)
                {
                    Social.ReportProgress("CgkI5cGT4tUGEAIQBw", 100.0f, (bool success) =>
                    {
                        // handle success or failure
                    });
                }

                if (DataManager.meters >= 1000000000)
                {
                    Social.ReportProgress("CgkI5cGT4tUGEAIQCA", 100.0f, (bool success) =>
                    {
                        // handle success or failure
                    });
                }

                if (DataManager.meters >= 1000000000000)
                {
                    Social.ReportProgress("CgkI5cGT4tUGEAIQCQ", 100.0f, (bool success) =>
                    {
                        // handle success or failure
                    });
                }
            }
        }
	}
}
