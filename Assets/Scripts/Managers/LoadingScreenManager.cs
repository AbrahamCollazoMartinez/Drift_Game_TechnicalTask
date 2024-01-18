using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadingScreenManager : Singleton<LoadingScreenManager>
{

	public Action<bool> showLoadingScreen = delegate { };

	bool initialSetup = false;

	private void Start()
	{
		if (!initialSetup)
		{
			initialSetup = true;
			this.gameObject.SetActive(false);
			showLoadingScreen += showLoading;
		}
	}

	public void showLoading(bool state)
	{
		this.gameObject.SetActive(state);
	}

	private void OnDestroy()
	{
		showLoadingScreen -= showLoading;

	}
}
