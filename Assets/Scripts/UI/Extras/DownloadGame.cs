using UnityEngine;
using System.Collections;

public class DownloadGame : MonoBehaviour
{
    public string marketURL;
    public string key;
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

    public void Visit()
    {
        Application.OpenURL(marketURL);

        if (!visited)
        {
            PlayerPrefs.SetInt(key, 1);
            DataManager.DoubleMeters();
            visited = true;
        }
    }
}
