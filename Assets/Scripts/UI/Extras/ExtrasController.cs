using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrasController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}
