using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStats : MonoBehaviour
{
    public GameObject statItemPrefab;

    public Color colorOdds;

    void Awake()
    {
        RefreshList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshList();
    }

    void CreateStat(int index, string title, string data)
    {
        GameObject item = Instantiate(statItemPrefab) as GameObject;
        item.transform.SetParent(transform, false);

        Text name = item.transform.Find("Title").GetComponent<Text>();
        Text stat = item.transform.Find("Stat").GetComponent<Text>();

        name.text = title;
        stat.text = data;

        Image image = item.gameObject.GetComponent<Image>();

        if (index % 2 != 0) { image.color = colorOdds; }
    }

    public void RefreshList()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        StatsManager statsManager = GameObject.FindObjectOfType<StatsManager>();

        CreateStat(0, "Current meters:", Utils.FloatToString(DataManager.meters));
        CreateStat(1, "Meters (this game):", Utils.FloatToString(statsManager.game_meters));
        CreateStat(2, "Meters (all time):", Utils.FloatToString(statsManager.total_meters));
        CreateStat(3, "Meters (user):", Utils.FloatToString(statsManager.user_meters));
        CreateStat(4, "Total time:", Utils.GetTimeDifference(statsManager.totalStart, DateTime.Now));
        CreateStat(5, "Game time:", Utils.GetTimeDifference(statsManager.gameStart, DateTime.Now));
        CreateStat(6, "Resets:", statsManager.resets + " RESETS");
    }
}
