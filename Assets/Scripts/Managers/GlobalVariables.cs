using UnityEngine;
using System;
using System.Collections;
public class GlobalVariables : Singleton<GlobalVariables>
{
	public CarBrain playerCar;
	public CarBrain playerCarPrefab;
	public Cars[] carsAccess;
	public GameObject[] accesoriesCar;
	public GameObject testObject;
}

[Serializable]
public class Cars
{
	public GameObject[] carsSameType;
}
