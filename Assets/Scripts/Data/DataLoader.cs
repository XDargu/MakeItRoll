using UnityEngine;
using System.Collections;

public class DataLoader : MonoBehaviour {

    void Awake()
    {
        DataManager.LoadMeters();
        DataManager.LoadStoreItems();
        DataManager.LoadUpgrades();
        DataManager.UpdateRollData();
        DataManager.UpdateStoreItemsMPS();
        DataManager.UpdateStoreItemsPrice();
        DataManager.UpdateTotalMPS();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
