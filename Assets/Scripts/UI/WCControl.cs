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

	// Use this for initialization
	void Start () {
        GameObject.Find("GestureManager").GetComponent<GestureManager>().receivers.Add(this);
        statsManager = GameObject.FindObjectOfType<StatsManager>();

        mImage = GetComponent<Image>();
        mRectTransform = GetComponent<RectTransform>();

        /*DataManager.WCOpen = PlayerPrefs.GetInt("WCOpen", 1) == 1;
        mySprite.SpriteName = DataManager.WCOpen ? "WC_open" : "WC_closed";*/
	}
	
	// Update is called once per frame
	void Update () {

        /*string mph = "";
        if ((toiletBar.fill == 100) || (!DataManager.WCOpen))
            mph += "[color #EF4C36]+0";
        else
            mph += "[color #513639]+" + Util.FloatToString(DataManager.toiletMPS * 3600f);
        mph += " M/HOUR[/color]";

        string totals = "[color #513639]" + (Util.FloatToString(DataManager.toiletCapacity * toiletBar.fill / 100f)) + "/" + Util.FloatToString(DataManager.toiletCapacity) + "[/color]";
        if (totals.Length > 16)
        {
            totals = "[color #513639]" + (Util.FloatToString(DataManager.toiletCapacity * toiletBar.fill / 100f)) + "/\n" + Util.FloatToString(DataManager.toiletCapacity) + "[/color]";
        }*/
        
        //WCLabel.Text = mph + "\n" + totals;
        //WCLabel.Text = Util.FloatToString(DataManager.totalMPS) + " m/s ";

        /*if (tabPanel.Position.y > 1f)
        {
            WCLabel.IsVisible = true;
        }
        else
        {
            WCLabel.IsVisible = false;
        }*/
	}

    private static Vector3[] WorldCorners = new Vector3[4];
    public static Bounds GetRectTransformBounds(RectTransform transform)
    {
        transform.GetWorldCorners(WorldCorners);
        Bounds bounds = new Bounds(WorldCorners[0], Vector3.zero);
        for (int i = 1; i < 4; ++i)
        {
            bounds.Encapsulate(WorldCorners[i]);
        }
        return bounds;
    }

    public void OnGesture(Gesture gesture)
    {
        if (gesture.type == GestureType.Tap)
        {
            Bounds myBounds = GetRectTransformBounds(mRectTransform);

            if (myBounds.Contains(gesture.position))
            {
                GetComponent<AudioSource>().PlayOneShot(flushSound);
                statsManager.ResetFill(true);
            }
        }
        else if (gesture.type == GestureType.Drag)
        {
            Bounds myBounds = GetRectTransformBounds(mRectTransform);

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

        /*if (tabPanel.Position.y > 0)
        {
            if (gesture.type == GestureType.Tap)
            {
                Vector3 wp = guiCamera.ScreenToWorldPoint(gesture.position);

                if (collider.bounds.Contains(wp))
                {
                    audio.PlayOneShot(flushSound);
                    statsManager.ResetFill(true);
                }
            }
            if (gesture.type == GestureType.Drag)
            {
                Vector3 wp = guiCamera.ScreenToWorldPoint(gesture.position);

                if (collider.bounds.Contains(wp))
                {
                    if (gesture.delta.y < 0)
                    {
                        // Abajo
                        if (DataManager.WCOpen)
                        {
                            mySprite.SpriteName = "WC_closed";
                            PlayerPrefs.SetInt("WCOpen", 0);
                            insideSprite = false;
                            audio.PlayOneShot(closeSound);
                            DataManager.WCOpen = false;
                        }
                    }
                    else
                    {
                        // Arriba
                        if (!DataManager.WCOpen)
                        {
                            mySprite.SpriteName = "WC_open";
                            PlayerPrefs.SetInt("WCOpen", 1);
                            insideSprite = false;
                            audio.PlayOneShot(openSound);
                            DataManager.WCOpen = true;
                        }
                    }
                }
            }
        }*/
    }
}
