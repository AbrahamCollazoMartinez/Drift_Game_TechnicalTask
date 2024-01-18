using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


//For purposes of time to finish the technical task i decided save the data into player prefs but i can connect it to a data base like firebase, playfab and SQL

public class SaveSystem : Singleton<SaveSystem>
{

	public void SaveCustomizationData()
	{
		var cd_JsonObj = new JSONObject(JsonUtility.ToJson(CustomizationManager.currentCustomization));

		PlayerPrefs.SetString("CarData", cd_JsonObj.ToString());
		PlayerPrefs.Save();
	}

	public void ReadCustomizationData()
	{
		CarCustomization generatedDataParsing = new CarCustomization();

		if (PlayerPrefs.HasKey("CarData"))
		{
			string cardata = PlayerPrefs.GetString("CarData");
			if (!string.IsNullOrEmpty(cardata))
			{

				generatedDataParsing = JsonConvert.DeserializeObject<CarCustomization>(cardata);
			}
		}

		CustomizationManager.currentCustomization = generatedDataParsing;
	}

	public void SaveStoreData(int driftcoins)
	{
		PlayerPrefs.SetInt("StoreData", driftcoins);
		PlayerPrefs.Save();
	}

	public int ReadStoreData()
	{
		int storeData = 0;
		if (PlayerPrefs.HasKey("StoreData"))
		{
			storeData = PlayerPrefs.GetInt("StoreData");

		}

		return storeData;
	}

	void OnApplicationQuit()
	{
		SaveCustomizationData();
	}

}
