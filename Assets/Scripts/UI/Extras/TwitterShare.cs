using UnityEngine;
using System.Collections;

public class TwitterShare : MonoBehaviour
{
    string marketURL = "https://play.google.com/store/apps/details?id=com.OverratedGames.MakeItRoll";
    string key = "twitterShare";
    bool visited;

    // Use this for initialization
    void Start()
    {
        visited = PlayerPrefs.GetInt(key, 0) == 1;
        if (DataManager.currentMaket == DataManager.Market.GooglePlay)
            marketURL = "https://play.google.com/store/apps/details?id=com.OverratedGames.MakeItRoll";
        if (DataManager.currentMaket == DataManager.Market.Amazon)
            marketURL = "http://www.amazon.com/gp/mas/dl/android?p=com.OverratedGames.MakeItRoll";

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Tweet()
    {
        string points = Utils.FloatToString(DataManager.meters);
        Application.OpenURL("https://twitter.com/intent/tweet?text=I'm%20making%20it%20roll%20with%20%23MakeItRoll%20and%20I%20made%20" + points + "%20meters.%20Can%20you%20beat%20me?&url=" + marketURL);

        if (!visited)
        {
            PlayerPrefs.SetInt(key, 1);
            DataManager.DoubleMeters();
            visited = true;
        }
    }
}
