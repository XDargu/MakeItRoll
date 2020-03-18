using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

    //public dfPanel eventWindow;
    //public dfPanel rootPanel;

	// Use this for initialization
	void Start () {
        DataManager.LoadEvents();
	}
	
	// Update is called once per frame
	void Update () {
        foreach (DataManager.Event ev in DataManager.events)
        {
            if (DataManager.eventData[ev.ID] == false)
            {
                if (ev.metersToPlay <= DataManager.meters)
                {
                    /*dfPanel window = Instantiate(eventWindow) as dfPanel;

                    rootPanel.AddControl(window);
                    window.Position = new Vector3(50, -50, 0);
                    window.Width = rootPanel.Width - 100;
                    window.ZOrder = 10;

                    EventWindow ew = window.GetComponent<EventWindow>();
                    ew.title = ev.title;
                    ew.description = ev.description;
                    ew.LoadData();*/
                    DataManager.FireEvent(ev.ID);
                    DataManager.SaveEvents();

                    //AndroidGoogleAnalytics.instance.SendEvent("Make It Roll events", "Fired " + ev.ID + " event", ev.title);
                }
            }
        }
	}
}
