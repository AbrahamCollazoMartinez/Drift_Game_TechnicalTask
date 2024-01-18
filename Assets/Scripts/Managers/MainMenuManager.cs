using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject[] panelsMenu;
	public static MainMenuManager instance;

	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		if (PhotonNetwork.IsConnected)
		{
			panelsMenu[0].transform.DOScale(1, 1f).SetEase(Ease.InOutQuint);
			SaveSystem.Instance.ReadCustomizationData();
			CustomizationManager.instance.UpdateUI();
			CustomizationManager.instance.UpdateCarShowing();

		}
	}

	public void ShowMainMenu()
	{
		panelsMenu[0].transform.DOScale(1, 1f).SetEase(Ease.InOutQuint);
		SaveSystem.Instance.ReadCustomizationData();
		CustomizationManager.instance.UpdateUI();
		CustomizationManager.instance.UpdateCarShowing();
	}

	public void SaveCustomization()
	{
		SaveSystem.Instance.SaveCustomizationData();
	}
}
