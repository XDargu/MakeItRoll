using UnityEngine;
using System.Collections;

public class DataLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DataManager.LoadMeters();
        DataManager.LoadStoreItems();
        DataManager.LoadUpgrades();
        DataManager.UpdateRollData();
        DataManager.UpdateStoreItemsMPS();
        DataManager.UpdateStoreItemsPrice();
        DataManager.UpdateTotalMPS();
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
