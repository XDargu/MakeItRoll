using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {

    public static bool playServicesLoaded = false;

    //public static Dictionary<string, bool> achievementState = new Dictionary<string,bool>();

    float counter = 0f;
    public float updateRate = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Logros
        /*if (playServicesLoaded)
        {
            counter += Time.deltaTime;
            if (counter >= updateRate)
            {
                counter = 0f;
                if (DataManager.meters >= 1)
                {
                    if (GooglePlayManager.instance.achievements["CgkI5cGT4tUGEAIQBQ"].state != GPAchievementState.STATE_UNLOCKED)
                    {
                        GooglePlayManager.instance.reportAchievementById("CgkI5cGT4tUGEAIQBQ");
                    }
                }

                if (DataManager.meters >= 100)
                {
                    if (GooglePlayManager.instance.achievements["CgkI5cGT4tUGEAIQBg"].state != GPAchievementState.STATE_UNLOCKED)
                    {
                        GooglePlayManager.instance.reportAchievementById("CgkI5cGT4tUGEAIQBg");
                    }
                }

                if (DataManager.meters >= 1000000)
                {
                    if (GooglePlayManager.instance.achievements["CgkI5cGT4tUGEAIQBw"].state != GPAchievementState.STATE_UNLOCKED)
                    {
                        GooglePlayManager.instance.reportAchievementById("CgkI5cGT4tUGEAIQBw");
                    }
                }

                if (DataManager.meters >= 1000000000)
                {
                    if (GooglePlayManager.instance.achievements["CgkI5cGT4tUGEAIQCA"].state != GPAchievementState.STATE_UNLOCKED)
                    {
                        GooglePlayManager.instance.reportAchievementById("CgkI5cGT4tUGEAIQCA");
                    }
                }

                if (DataManager.meters >= 1000000000000)
                {
                    if (GooglePlayManager.instance.achievements["CgkI5cGT4tUGEAIQCQ"].state != GPAchievementState.STATE_UNLOCKED)
                    {
                        GooglePlayManager.instance.reportAchievementById("CgkI5cGT4tUGEAIQCQ");
                    }
                }
            }
        }*/
	}
}
