using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour
{
    public static void UpdateStoreItemLabels()
    {
        StoreItem[] items = GameObject.FindObjectsOfType<StoreItem>() as StoreItem[];
        foreach (StoreItem storeItem in items)
        {
            storeItem.UpdateData();
        }
    }

    public static void UpdateUpgrades()
    {
        FillUpgrades fillUpgrades = Object.FindObjectOfType<FillUpgrades>();
        if (fillUpgrades)
        {
            fillUpgrades.RefreshList();
        }
    }

    public static void UpdateStoreItems()
    {
        FillStore fillStore = Object.FindObjectOfType<FillStore>();
        if (fillStore)
        {
            fillStore.RefreshList();
        }
    }

    // Control de actualizaciones
    public static List<string> availableUpdates = new List<string>();
    static bool firstLoad = true;

    public static void ResetNotificationSystem()
    {
        availableUpdates.Clear();
        firstLoad = true;
    }

    public static void RemoveAvailableUpdate(string ID)
    {
        availableUpdates.Remove(ID);
    }

    public static void ClearUpgradeNotification()
    {
        GameObject notificationSprite = GameObject.Find("UpgradesNotificationSprite");
        Image notificationImage = notificationSprite.GetComponent<Image>();
        notificationImage.enabled = false;
        if (PlayerPrefs.HasKey("notifications"))
        {
            PlayerPrefs.DeleteKey("notifications");
        }
    }

    public static void UpdateNotifications(List<string> currentUpdates)
    {
        bool update = false;

        if (firstLoad)
        {
            if (PlayerPrefs.HasKey("notifications"))
            {
                update = true;
            }
            firstLoad = false;
        }
        else
        {
            foreach (string upgrade in currentUpdates)
            {
                if (!availableUpdates.Contains(upgrade))
                {
                    update = true;
                    break;
                }
            }
        }

        if (update)
        {
            GameObject notificationSprite = GameObject.Find("UpgradesNotificationSprite");
            Image notificationImage = notificationSprite.GetComponent<Image>();
            notificationImage.enabled = true;
            PlayerPrefs.SetInt("notifications", 1);
            Debug.Log("Blink");
        }

        availableUpdates = currentUpdates;
    }

    public static void UpdateToiletPaper()
    {
        MoveDown toiletPaper = GameObject.FindObjectOfType<MoveDown>();
        toiletPaper.rollLongitude = DataManager.rollCapacity;
        toiletPaper.maxSpeed = (15 + DataManager.userMPSSumIncrement) * DataManager.userMPSMulIncrement;
    }

}
