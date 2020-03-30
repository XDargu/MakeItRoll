using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LabelMetersPerSecond : MonoBehaviour
{
    Text m_Text;

    // Use this for initialization
    void Start()
    {
        m_Text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float globalMultiplier = 1f;
        if (DataManager.IsDoublePaperPurchased)
        {
            globalMultiplier *= 2;
        }
        if (DataManager.IsHappyHour)
        {
            globalMultiplier *= 100;
        }
        if (DataManager.IsVideoRewardActive)
        {
            globalMultiplier *= 1.5f;
        }
        string multiplier = "";
        if (globalMultiplier > 1)
        {
            multiplier = "(x " + globalMultiplier + ")";
        }
        m_Text.text = Utils.FloatToString(DataManager.totalMPS + DataManager.userMPS) + " m/s " + multiplier;
    }
}
