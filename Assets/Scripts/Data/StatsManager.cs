using UnityEngine;
using System.Collections;
using System;

public class StatsManager : MonoBehaviour {

    public int saveRate = 10;
    float counter = 0f;

    string TOTAL_METERS = "total_meters";
    string GAME_METERS = "game_meters";
    string USER_METERS = "user_meters";
    string TOTAL_START = "total_start";
    string GAME_START = "game_start";
    string GOLDEN_ROLLS = "golden_rolls";
    string RESETS = "resets";
    // WC
    string LAST_ACCESS_DATE = "last_access_date";
    string LAST_ACCESS_TOILET = "last_access_toilet";

    public float total_meters;
    public float game_meters;
    public float user_meters;
    public int golden_rolls;
    public int resets;

    public DateTime totalStart;
    public DateTime gameStart;

    public DateTime lastAccessDate;

    public ToiletBar toiletBar;

	// Use this for initialization
	void Start () {

        // Control del WC
        DataManager.WCOpen = PlayerPrefs.GetInt("WCOpen", 1) == 1;

        lastAccessDate = DateTime.Parse(PlayerPrefs.GetString(LAST_ACCESS_DATE, DateTime.Now.ToString()));
        float lastFill = PlayerPrefs.GetFloat(LAST_ACCESS_TOILET, 0) * 100f / DataManager.toiletCapacity;

        if (DataManager.WCOpen)
        {            
            TimeSpan elapsedTime = DateTime.Now.Subtract(lastAccessDate);
            float produced = (float)elapsedTime.TotalSeconds * DataManager.toiletMPS;
            float onePercent = DataManager.toiletCapacity / 100f;
            float fillPercent = produced / onePercent;
            float finalFill = lastFill + fillPercent;

            toiletBar.fill = Math.Min(100, finalFill);
        }
        else
        {
            toiletBar.fill = Math.Min(100, lastFill);
        }
        
        
        total_meters = PlayerPrefs.GetFloat(TOTAL_METERS, 0);
        game_meters = PlayerPrefs.GetFloat(GAME_METERS, 0);
        user_meters = PlayerPrefs.GetFloat(USER_METERS, 0);

        totalStart = DateTime.Parse(PlayerPrefs.GetString(TOTAL_START, DateTime.Now.ToString()));
        gameStart = DateTime.Parse(PlayerPrefs.GetString(GAME_START, DateTime.Now.ToString()));

        golden_rolls = PlayerPrefs.GetInt(GOLDEN_ROLLS, 0);

        resets = PlayerPrefs.GetInt(RESETS, 0);

        // Happy hour
        if (PlayerPrefs.HasKey("x100active"))
        {
            DataManager.IsHappyHour = true;
            DataManager.happyHourTime = DateTime.Parse(PlayerPrefs.GetString("x100StartTime", DateTime.Now.ToString()));
        }
	}

    float total_counter = 0f;
	
	// Update is called once per frame
	void Update () {

        // WC
        if (DataManager.WCOpen)
        {
            toiletBar.fill += (DataManager.toiletMPS * Time.deltaTime * 100) / DataManager.toiletCapacity;
            if (toiletBar.fill > 100) { toiletBar.fill = 100; }
        }

        // Happy hour
        if (DataManager.IsHappyHour)
        {
            if (DateTime.Now.Subtract(DataManager.happyHourTime).TotalMinutes > 60)
            {
                DataManager.DisableHappyHour();
            }
        }

        float mps = (DataManager.userMPS + DataManager.totalMPS) * Time.deltaTime;
        float globalMultiplier = 1f;
        if (DataManager.IsDoublePaperPurchased)
        {
            globalMultiplier *= 2;
        }
        if (DataManager.IsHappyHour)
        {
            globalMultiplier *= 100;
        }

        DataManager.meters += mps * globalMultiplier;

        total_counter += mps;
        if (total_counter >= 0.1f)
        {
            total_meters += total_counter;
            game_meters += total_counter;
            total_counter -= 0.1f;
        }        
        user_meters += DataManager.userMPS * Time.deltaTime;
        

        counter += Time.deltaTime;
        if (counter >= saveRate)
        {
            PlayerPrefs.SetFloat(TOTAL_METERS, total_meters);
            PlayerPrefs.SetFloat(GAME_METERS, game_meters);
            PlayerPrefs.SetFloat(USER_METERS, user_meters);

            PlayerPrefs.SetString(TOTAL_START, totalStart.ToString());
            PlayerPrefs.SetString(GAME_START, gameStart.ToString());

            PlayerPrefs.SetString(LAST_ACCESS_DATE, DateTime.Now.ToString());
            PlayerPrefs.SetFloat(LAST_ACCESS_TOILET, toiletBar.fill * DataManager.toiletCapacity / 100f);

            PlayerPrefs.SetInt(GOLDEN_ROLLS, golden_rolls);

            DataManager.SaveMeters();
            counter = 0f;
        }
	}

    public void ResetGame()
    {
        resets++;
        PlayerPrefs.SetInt(RESETS, resets);
        game_meters = 0f;
        gameStart = DateTime.Now;
        PlayerPrefs.SetFloat(GAME_METERS, 0f);
        PlayerPrefs.SetString(GAME_START, DateTime.Now.ToString());
    }

    public void AddMeters(float meters)
    {
        total_meters += meters;
        game_meters += meters;
    }

    public void ResetFill(bool givePoints)
    {
        if (givePoints)
        {
            // Obtener cuántos metros se ganan con el fill actual
            float earnedMeters = DataManager.toiletCapacity * toiletBar.fill / 100f;
            DataManager.meters += earnedMeters;
            AddMeters(earnedMeters);
        }

        PlayerPrefs.SetString(LAST_ACCESS_DATE, DateTime.Now.ToString());
        PlayerPrefs.SetFloat(LAST_ACCESS_TOILET, 0);
        toiletBar.fill = 0f;
    }

    public void SetFill(float amount)
    {
        toiletBar.fill = amount;
        PlayerPrefs.SetString(LAST_ACCESS_DATE, DateTime.Now.ToString());
        PlayerPrefs.SetFloat(LAST_ACCESS_TOILET, toiletBar.fill * DataManager.toiletCapacity / 100f);
    }

    public float GetFill()
    {
        return toiletBar.fill;
    }
}
