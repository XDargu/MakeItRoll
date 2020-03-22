﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class InAppProductItem : MonoBehaviour
{
    public Text name;
    public Text description;
    public Text price;
    public Image icon;
    public Button buyButton;

    private string m_ProductID;
    private Action<string> m_PurchaseCallback;

    public void SetProduct(Product p, Action<string> purchaseCallback)
    {
        name.text = p.metadata.localizedTitle;
        description.text = p.metadata.localizedDescription;
        price.text = p.metadata.localizedPriceString;

        m_ProductID = p.definition.id;
        m_PurchaseCallback = purchaseCallback;

        buyButton.interactable = p.availableToPurchase;
    }

    // Start is called before the first frame update
    void Start()
    {
        buyButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Purchase()
    {
        if (m_PurchaseCallback != null && !string.IsNullOrEmpty(m_ProductID))
        {
            m_PurchaseCallback(m_ProductID);
        }
    }
}
