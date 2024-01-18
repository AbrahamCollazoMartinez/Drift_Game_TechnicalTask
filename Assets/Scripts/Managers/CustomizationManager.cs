using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CustomizationManager : MonoBehaviour
{
	[SerializeField] private GameObject carContainer;
	[SerializeField] private Toggle[] aditionsCar;

	public static CarCustomization currentCustomization;
	public static CustomizationManager instance;


	private void Awake()
	{
		currentCustomization = new CarCustomization();
		instance = this;
	}

	public void NextCar()
	{
		currentCustomization.car = currentCustomization.car + 1;

		if (currentCustomization.car > GlobalVariables.Instance.carsAccess.Count() - 1)
		{
			currentCustomization.car = 0;
		}

		UpdateCarShowing();


	}

	public void PreviousCar()
	{
		currentCustomization.car = currentCustomization.car - 1;

		if (currentCustomization.car < 0)
		{
			currentCustomization.car = GlobalVariables.Instance.carsAccess.Count() - 1;
		}

		UpdateCarShowing();

	}

	public void NextColor()
	{
		currentCustomization.colorCar = currentCustomization.colorCar + 1;

		if (currentCustomization.colorCar > GlobalVariables.Instance.carsAccess[currentCustomization.car].carsSameType.Count() - 1)
		{
			currentCustomization.colorCar = 0;
		}

		UpdateCarShowing();

	}

	public void PreviousColor()
	{
		currentCustomization.colorCar = currentCustomization.colorCar - 1;

		if (currentCustomization.colorCar < 0)
		{
			currentCustomization.colorCar = GlobalVariables.Instance.carsAccess[currentCustomization.car].carsSameType.Count() - 1;
		}

		UpdateCarShowing();

	}

	public void HasEngine(bool state)
	{
		currentCustomization.hasEngine = state;
		UpdateCarShowing();

	}

	public void HasLights(bool state)
	{
		currentCustomization.hasLights = state;
		UpdateCarShowing();

	}


	public void UpdateCarShowing()
	{
		foreach (Transform item in carContainer.transform)
		{
			
			Destroy(item.gameObject);

		}

		GameObject carSelected = Instantiate(GlobalVariables.Instance.carsAccess[currentCustomization.car].carsSameType[currentCustomization.colorCar]);
		carSelected.transform.parent = carContainer.transform;
		carSelected.transform.localPosition = Vector3.zero;
		carSelected.transform.localRotation = Quaternion.Euler(Vector3.zero);

		if (currentCustomization.hasEngine)
		{
			GameObject engine = Instantiate(GlobalVariables.Instance.accesoriesCar[0]);
			engine.transform.parent = carSelected.transform;
			engine.transform.localPosition = Vector3.zero;
			engine.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}

		if (currentCustomization.hasLights)
		{
			GameObject lights = Instantiate(GlobalVariables.Instance.accesoriesCar[1]);
			lights.transform.parent = carSelected.transform;
			lights.transform.localPosition = Vector3.zero;
			lights.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		SaveSystem.Instance.SaveCustomizationData();
	}

	public void UpdateUI()
	{
		aditionsCar[0].isOn = currentCustomization.hasEngine;
		aditionsCar[1].isOn = currentCustomization.hasLights;
	}

}

[Serializable]
public class CarCustomization
{
	public int car;
	public int colorCar;
	public bool hasEngine;
	public bool hasLights;
}

