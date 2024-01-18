using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Redcode.Extensions;

public class CarCustomizationAccess : MonoBehaviour
{

	[SerializeField] private GameObject cameraPlayer, motor, carCollider;
	[SerializeField] private CarBrain controlsPlayer;
	private WheelController wheelBrainPlayer;
	private CarCustomization customizationThisCar;

	public void SetCustomization(CarCustomization data)
	{
		customizationThisCar = data;
		SpawnCar();
	}


	public void SpawnCar()
	{

		GameObject carSelected = PhotonNetwork.Instantiate(GlobalVariables.Instance.carsAccess[customizationThisCar.car].carsSameType[customizationThisCar.colorCar].name, this.transform.position, this.transform.rotation);
		wheelBrainPlayer = carSelected.GetComponent<WheelController>();

		ParentingSearcher parentingsystem = carSelected.GetComponent<ParentingSearcher>();
		parentingsystem.EnableParentingSystem();

		if (customizationThisCar.hasEngine)
		{
			GameObject engine = PhotonNetwork.Instantiate(GlobalVariables.Instance.accesoriesCar[0].name, this.transform.position, this.transform.rotation);

			parentingsystem = engine.GetComponent<ParentingSearcher>();
			parentingsystem.EnableParentingSystem();
		}

		if (customizationThisCar.hasLights)
		{
			GameObject lights = PhotonNetwork.Instantiate(GlobalVariables.Instance.accesoriesCar[1].name, this.transform.position, this.transform.rotation);


			parentingsystem = lights.GetComponent<ParentingSearcher>();
			parentingsystem.EnableParentingSystem();
		}

		SetUpSystems();
	}

	private void SetUpSystems()
	{
		cameraPlayer.SetActive(true);
		motor.SetActive(true);
		carCollider.SetActive(true);
		//controlsPlayer.enabled = true;
		wheelBrainPlayer.enabled = true;
	}
}
