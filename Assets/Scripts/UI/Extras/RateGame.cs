using UnityEngine;
using System.Collections;

public class RateGame : MonoBehaviour
{
    string rateURL = "market://details?id=com.example.android";
    string key = "rateGame";
    bool visited;

    // Use this for initialization
    void Start()
    {
        visited = PlayerPrefs.GetInt(key, 0) == 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Rate()
    {
        Application.OpenURL(rateURL);

        if (!visited)
        {
            PlayerPrefs.SetInt(key, 1);
            DataManager.DoubleMeters();
            visited = true;
        }
    }
}
