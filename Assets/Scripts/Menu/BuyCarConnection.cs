using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCarConnection : MonoBehaviour
{
	[SerializeField] private int requiredDriftcoins;


	public void TryToPurchase()
	{
		StoreManager.instance.tryToPruchaseCar(requiredDriftcoins);
	}
}
