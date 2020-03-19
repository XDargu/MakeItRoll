using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelMeters : MonoBehaviour
{
    Text mText;
    float value;

    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<Text>();
        value = DataManager.meters;
    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.Lerp(value, DataManager.meters, 0.3f);
        if (value > 1000) { value = Mathf.Floor(value); }
        mText.text = "" + Utils.FloatToString(value) + ""; ;
    }
}
