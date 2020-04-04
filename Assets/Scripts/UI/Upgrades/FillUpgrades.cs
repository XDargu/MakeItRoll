using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FillUpgrades : MonoBehaviour
{
    public GameObject contentObject;
    public GameObject upgradeItemPrefab;
    VerticalLayoutGroup m_VerticalLayourGroup;

    public Color colorOdds;

    struct SortingData
    {
        public float price;
        public GameObject gameObject;

        public SortingData(float price, GameObject gameObject)
        {
            this.price = price;
            this.gameObject = gameObject;
        }
    }

    void Awake()
    {
        m_VerticalLayourGroup = gameObject.GetComponent<VerticalLayoutGroup>();

        DataManager.LoadUpgrades();
        DataManager.LoadStoreItems();
        DataManager.UpdateToiletData(false);
        DataManager.UpdateRollData();
    }

    // Use this for initialization
    void Start()
    {
        RefreshList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject AddUpgrade(string ID, float price, string name, string description)
    {
        GameObject item = Instantiate(upgradeItemPrefab) as GameObject;

        Text nameLabel = item.transform.Find("Name").GetComponent<Text>();
        Text descriptionLabel = item.transform.Find("Description").GetComponent<Text>();
        Text priceLabel = item.transform.Find("Price").GetComponent<Text>();
        Image icon = item.transform.Find("Icon").GetComponent<Image>();
        icon.sprite = Resources.Load<Sprite>("Upgrades/" + ID);

        UpgradeItem upgradeItem = item.gameObject.GetComponent<UpgradeItem>();
        upgradeItem.ID = ID;
        upgradeItem.price = price;

        nameLabel.text = name;
        descriptionLabel.text = description;
        priceLabel.text = Utils.FloatToString(price) + " m";

        //item.tag = DataManager.rollUpgrades[i].price;
        item.name = ID;
        return item;
    }

    public void RefreshList()
    {
        List<string> updateList = new List<string>();
        List<SortingData> sortingData = new List<SortingData>();

        if (contentObject)
        {
            foreach (Transform child in contentObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        bool skipUpgrade = false;

        // De rollo
        for (int i = 0; i < DataManager.rollUpgrades.Length; i++)
        {
            skipUpgrade = false;

            if (DataManager.upgradesData.ContainsKey(DataManager.rollUpgrades[i].ID))
            {
                // No tener la mejora
                if (DataManager.upgradesData[DataManager.rollUpgrades[i].ID] == true)
                {
                    skipUpgrade = true;
                }
            }

            // No se tiene la mejora anterior
            if (DataManager.rollUpgrades[i].prevID != null)
            {
                if (DataManager.upgradesData[DataManager.rollUpgrades[i].prevID] == false)
                {
                    skipUpgrade = true;
                }
            }

            if (!skipUpgrade)
            {
                if (contentObject)
                {
                    GameObject upgrade = AddUpgrade(DataManager.rollUpgrades[i].ID, DataManager.rollUpgrades[i].price, DataManager.rollUpgrades[i].name, DataManager.rollUpgrades[i].description);
                    sortingData.Add(new SortingData(DataManager.rollUpgrades[i].price, upgrade));
                }

                updateList.Add(DataManager.rollUpgrades[i].ID);
            }
        }

        // De WC
        for (int i = 0; i < DataManager.toiletUpgrades.Length; i++)
        {
            skipUpgrade = false;

            if (DataManager.upgradesData.ContainsKey(DataManager.toiletUpgrades[i].ID))
            {
                // No tener la mejora
                if (DataManager.upgradesData[DataManager.toiletUpgrades[i].ID] == true)
                {
                    skipUpgrade = true;
                }
            }

            // No se tiene la mejora anterior
            if (DataManager.toiletUpgrades[i].prevID != null)
            {
                if (DataManager.upgradesData[DataManager.toiletUpgrades[i].prevID] == false)
                {
                    skipUpgrade = true;
                }
            }

            if (!skipUpgrade)
            {
                if (contentObject)
                {
                    GameObject upgrade = AddUpgrade(DataManager.toiletUpgrades[i].ID, DataManager.toiletUpgrades[i].price, DataManager.toiletUpgrades[i].name, DataManager.toiletUpgrades[i].description);
                    sortingData.Add(new SortingData(DataManager.toiletUpgrades[i].price, upgrade));
                }

                updateList.Add(DataManager.toiletUpgrades[i].ID);
            }
        }

        // De items
        for (int i = 0; i < DataManager.storeItemUpgrades.Length; i++)
        {
            skipUpgrade = false;
            if (DataManager.upgradesData.ContainsKey(DataManager.storeItemUpgrades[i].ID))
            {
                // No tener la mejora
                if (DataManager.upgradesData[DataManager.storeItemUpgrades[i].ID] == true)
                {
                    skipUpgrade = true;
                }
            }

            // No se ha cumplido la condición para desbloquearse
            if (DataManager.storeItemUpgrades[i].storeItemsNeeded > DataManager.storeItemsData[DataManager.storeItemUpgrades[i].storeItemID])
            {
                skipUpgrade = true;
            }

            if (!skipUpgrade)
            {
                if (contentObject)
                {
                    GameObject upgrade = AddUpgrade(DataManager.storeItemUpgrades[i].ID, DataManager.storeItemUpgrades[i].price, DataManager.storeItemUpgrades[i].name, DataManager.storeItemUpgrades[i].description);
                    sortingData.Add(new SortingData(DataManager.storeItemUpgrades[i].price, upgrade));
                }

                updateList.Add(DataManager.storeItemUpgrades[i].ID);
            }
        }

        // Golden
        for (int i = 0; i < DataManager.goldenUpgrades.Length; i++)
        {
            skipUpgrade = false;

            if (DataManager.upgradesData.ContainsKey(DataManager.goldenUpgrades[i].ID))
            {
                // No tener la mejora
                if (DataManager.upgradesData[DataManager.goldenUpgrades[i].ID] == true)
                {
                    skipUpgrade = true;
                }
            }

            // No se tiene la mejora anterior
            if (DataManager.goldenUpgrades[i].prevID != null)
            {
                if (DataManager.upgradesData[DataManager.goldenUpgrades[i].prevID] == false)
                {
                    skipUpgrade = true;
                }
            }

            if (!skipUpgrade)
            {
                if (contentObject)
                {
                    GameObject upgrade = AddUpgrade(DataManager.goldenUpgrades[i].ID, DataManager.goldenUpgrades[i].price, DataManager.goldenUpgrades[i].name, DataManager.goldenUpgrades[i].description);
                    sortingData.Add(new SortingData(DataManager.goldenUpgrades[i].price, upgrade));
                }
                
                updateList.Add(DataManager.goldenUpgrades[i].ID);
            }
        }

        GUIManager.UpdateNotifications(updateList);

        // Ordenar
        // Array temporal
        if (contentObject)
        {
            sortingData.Sort(delegate (SortingData item1, SortingData item2)
            {
                return item1.price.CompareTo(item2.price);
            });

            // Recorrer, cambiar ZOrder y cambiar color
            for (int i = 0; i < sortingData.Count; i++)
            {
                sortingData[i].gameObject.transform.SetParent(contentObject.transform, false);
                if (i % 2 != 0)
                {
                    Image image = sortingData[i].gameObject.GetComponent<Image>();
                    image.color = colorOdds;
                }
            }
        }
    }
}
