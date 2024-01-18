using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		Transform positionRace = PositionsRaceAccess.instance.GetPositionAvaliable();
		GameObject carPlayerSpawn = PhotonNetwork.Instantiate(GlobalVariables.Instance.playerCarPrefab.name, positionRace.position, positionRace.rotation);


		carPlayerSpawn.transform.position = positionRace.position;
		carPlayerSpawn.transform.rotation = positionRace.rotation;

		CarBrain brainControllerPlayer = carPlayerSpawn.GetComponent<CarBrain>();
		GlobalVariables.Instance.playerCar = brainControllerPlayer;

		CarCustomizationAccess accessCustomization = carPlayerSpawn.GetComponent<CarCustomizationAccess>();
		accessCustomization.SetCustomization(CustomizationManager.currentCustomization);

		LoadingScreenManager.Instance.showLoading(false);
	}


}
