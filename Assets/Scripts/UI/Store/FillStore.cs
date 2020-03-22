using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStore : MonoBehaviour
{
    public GameObject storeItemPrefab;

    public Color colorOdds;

    void Awake()
    {
        DataManager.LoadUpgrades();
        DataManager.LoadStoreItems();
        DataManager.UpdateToiletData(false);
        DataManager.UpdateRollData();
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshList()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < DataManager.storeItems.Length; i++)
        {
            GameObject item = Instantiate(storeItemPrefab) as GameObject;
            item.transform.SetParent(transform, false);

            Image image = item.gameObject.GetComponent<Image>();

            if (i % 2 != 0) { image.color = colorOdds; }

            Text name = item.transform.FindChild("Name").GetComponent<Text>();
            Text description = item.transform.FindChild("Description").GetComponent<Text>();
            Text data = item.transform.FindChild("Data").GetComponent<Text>();
            Text amount = item.transform.FindChild("AmountSprite").FindChild("Amount").GetComponent<Text>();
            Text price = item.transform.FindChild("Price").GetComponent<Text>();

            StoreItem storeItem = item.gameObject.GetComponent<StoreItem>();
            storeItem.ID = DataManager.storeItems[i].ID;
            storeItem.amountLabel = amount;
            storeItem.basePrice = DataManager.storeItems[i].price;
            storeItem.priceLabel = price;
            storeItem.dataLabel = data;
            storeItem.increment = DataManager.storeItems[i].baseIncrement;
            storeItem.icon.sprite = Resources.Load<Sprite>("Items/" + DataManager.storeItems[i].ID);

            name.text = DataManager.storeItems[i].name;
            description.text = DataManager.storeItems[i].description;
            data.text = "+" + Utils.FloatToString(DataManager.storeItemsMPS[DataManager.storeItems[i].ID]) + " m/s";
            amount.text = "" + DataManager.storeItemsData[DataManager.storeItems[i].ID];
            price.text = Utils.FloatToString(Mathf.Floor(DataManager.storeItemsPrice[DataManager.storeItems[i].ID])) + " m";

            item.name = "Store Item" + DataManager.storeItems[i].ID;
        }
    }
}
