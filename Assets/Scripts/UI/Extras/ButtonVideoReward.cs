using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVideoReward : MonoBehaviour
{
    private Button m_Button;

    // Start is called before the first frame update
    void Start()
    {
        m_Button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Button.interactable = !DataManager.IsVideoRewardActive;
    }
}
