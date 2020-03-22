using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpgradeItem : MonoBehaviour
{
    public string ID;
    public float price;
    public Image icon;
    public Button buyButton;

    // Use this for initialization
    void Start()
    {
        UpdateButton();
    }

    void UpdateButton()
    {
        if (DataManager.meters >= Mathf.Floor(price))
        {
            if (!buyButton.interactable)
                buyButton.interactable = true;
        }
        else
        {
            if (buyButton.interactable)
                buyButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButton();
    }

    public void Buy()
    {
        if (Mathf.Floor(DataManager.meters) >= price)
        {
            AudioSource parentAudioSource = transform.parent.gameObject.GetComponent<AudioSource>();
            parentAudioSource.Play();

            DataManager.meters -= price;
            DataManager.upgradesData[ID] = true;

            DataManager.UpdateStoreItemsPrice();
            DataManager.UpdateStoreItemsMPS();

            DataManager.UpdateRollData();
            DataManager.UpdateToiletData(true);
            DataManager.UpdateTotalMPS();

            DataManager.SaveUpgrades();
            DataManager.SaveMeters();

            GUIManager.UpdateStoreItemLabels();
            GUIManager.RemoveAvailableUpdate(ID);
            GUIManager.UpdateUpgrades();
            GUIManager.UpdateToiletPaper();

            // Destruir el panel y sus hijos
            Destroy(gameObject);
        }
    }
}

