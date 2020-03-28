using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStats : MonoBehaviour
{
    public GameObject statItemPrefab;

    public Color colorOdds;

    public float updateRate = 1.0f;
    float counter = 0.0f;

    void Awake()
    {
        RefreshList();
    }

    // Start is called before the first frame update
    void Start()
    {
        counter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > updateRate)
        {
            RefreshList();
            counter = 0.0f;
        }
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
        Transform achievementsTransform = null;
        foreach (Transform child in transform)
        {
            if (child.name != "AchievementItem")
            {
                GameObject.Destroy(child.gameObject);
            }
            else
            {
                achievementsTransform = child;
            }
        }

        StatsManager statsManager = GameObject.FindObjectOfType<StatsManager>();

        CreateStat(0, "Current meters:", Utils.FloatToString(DataManager.meters));
        CreateStat(1, "Meters (this game):", Utils.FloatToString(statsManager.game_meters));
        CreateStat(2, "Meters (all time):", Utils.FloatToString(statsManager.total_meters));
        CreateStat(3, "Meters (user):", Utils.FloatToString(statsManager.user_meters));
        CreateStat(4, "Total time:", Utils.GetTimeDifference(statsManager.totalStart, DateTime.Now));
        CreateStat(5, "Game time:", Utils.GetTimeDifference(statsManager.gameStart, DateTime.Now));
        CreateStat(6, "Resets:", statsManager.resets + " RESETS");

        // Move achievement item to the end
        if (achievementsTransform != null)
        {
            achievementsTransform.SetSiblingIndex(transform.childCount - 1);
        }
    }
}
