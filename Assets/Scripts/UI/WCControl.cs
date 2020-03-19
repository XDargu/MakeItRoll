using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WCControl : MonoBehaviour {

    Image mImage;
    RectTransform mRectTransform;

    public AudioClip closeSound;
    public AudioClip openSound;
    public AudioClip flushSound;

    public Text perHourLabel;
    public Text totalLabel;

    StatsManager statsManager;
    public ToiletBar toiletBar;

    public Sprite toiletOpen;
    public Sprite toiletClosed;

    public Color normalText;
    public Color alertText;

	// Use this for initialization
	void Start () {
        GameObject.Find("GestureManager").GetComponent<GestureManager>().receivers.Add(this);
        statsManager = GameObject.FindObjectOfType<StatsManager>();

        mImage = GetComponent<Image>();
        mRectTransform = GetComponent<RectTransform>();

        DataManager.WCOpen = PlayerPrefs.GetInt("WCOpen", 1) == 1;
        mImage.sprite = DataManager.WCOpen ? toiletOpen : toiletClosed;
	}
	
	// Update is called once per frame
	void Update () {

        string mph = "";
        if ((toiletBar.fill == 100) || (!DataManager.WCOpen))
        {
            mph = "0 M/HOUR";
            perHourLabel.color = alertText;
        }
        else
        {
            mph = Utils.FloatToString(DataManager.toiletMPS * 3600f) + " M/HOUR";
            perHourLabel.color = normalText;
        }

        perHourLabel.text = mph;

        string totals = (Utils.FloatToString(DataManager.toiletCapacity * toiletBar.fill / 100f)) + "/" + Utils.FloatToString(DataManager.toiletCapacity);
        if (totals.Length > 16)
        {
            totals = (Utils.FloatToString(DataManager.toiletCapacity * toiletBar.fill / 100f)) + "/\n" + Utils.FloatToString(DataManager.toiletCapacity);
        }

        totalLabel.text = totals;
        
        //WCLabel.Text = mph + "\n" + totals;
        //WCLabel.Text = Util.FloatToString(DataManager.totalMPS) + " m/s ";
	}

    public void OnGesture(Gesture gesture)
    {
        if (gesture.type == GestureType.Tap)
        {
            Bounds myBounds = Utils.GetRectTransformBounds(mRectTransform);

            if (myBounds.Contains(gesture.position))
            {
                GetComponent<AudioSource>().PlayOneShot(flushSound);
                statsManager.ResetFill(true);
            }
        }
        else if (gesture.type == GestureType.Drag)
        {
            Bounds myBounds = Utils.GetRectTransformBounds(mRectTransform);

            if (myBounds.Contains(gesture.position))
            {
                if (gesture.delta.y < 0)
                {
                    // Abajo
                    if (DataManager.WCOpen)
                    {
                        mImage.sprite = toiletClosed;
                        PlayerPrefs.SetInt("WCOpen", 0);
                        GetComponent<AudioSource>().PlayOneShot(closeSound);
                        DataManager.WCOpen = false;
                    }
                }
                else
                {
                    // Arriba
                    if (!DataManager.WCOpen)
                    {
                        mImage.sprite = toiletOpen;
                        PlayerPrefs.SetInt("WCOpen", 1);
                        GetComponent<AudioSource>().PlayOneShot(openSound);
                        DataManager.WCOpen = true;
                    }
                }
            }
        }
    }
}
