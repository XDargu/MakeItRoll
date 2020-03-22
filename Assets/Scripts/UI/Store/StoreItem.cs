using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{

    public float basePrice;
    public float increment;
    public string ID;
    public Text amountLabel;
    public Text priceLabel;
    public Text dataLabel;
    public Button buyButton;
    public Image icon;

    AudioSource m_AudioSource;

    void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        UpdateButton();
    }

    void UpdateButton()
    {
        float realPrice = DataManager.GetStoreItemPrice(basePrice, DataManager.storeItemsData[ID]);
        if (DataManager.meters >= Mathf.Floor(realPrice))
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
        int oldAmount = DataManager.storeItemsData[ID];
        float realPrice = DataManager.GetStoreItemPrice(basePrice, oldAmount);
        realPrice = Mathf.Floor(realPrice);

        if (DataManager.meters >= realPrice)
        {
            m_AudioSource.Play();
            DataManager.storeItemsData[ID] = oldAmount + 1;
            DataManager.meters -= realPrice;

            DataManager.UpdateStoreItemsPrice();
            DataManager.UpdateStoreItemsMPS();
            DataManager.UpdateTotalMPS();
            DataManager.SaveStoreItems();
            DataManager.SaveMeters();

            UpdateData();
        }
    }

    public void UpdateData()
    {
        amountLabel.text = DataManager.storeItemsData[ID] + "";
        priceLabel.text = "" + Utils.FloatToString(Mathf.Floor(DataManager.storeItemsPrice[ID])) + " m";
        dataLabel.text = "+" + Utils.FloatToString(DataManager.storeItemsMPS[ID]) + " m/s";
    }
}

