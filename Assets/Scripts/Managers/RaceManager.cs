using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using BrunoMikoski.TextJuicer;
using DG.Tweening;

public class RaceManager : MonoBehaviour
{
	[SerializeField] private Timer startTimer;
	[SerializeField] private Timer raceTimer;
	[SerializeField] private GameObject gameFinishedPanel;
	[SerializeField] private TMP_TextJuicer totalDriftPoints, cashEarned;
	[SerializeField] private CanvasGroup fadeMessage;
	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);
		StartRace();
	}

	[ContextMenu("Start Race")]
	public void StartRace()
	{
		startTimer.gameObject.SetActive(true);
		startTimer.StartTimer();
	}

	public void LetPlayerMove()
	{
		GlobalVariables.Instance.playerCar.enabled = true;
	}

	public void RaceFinish()
	{
		fadeMessage.alpha = 1;
		gameFinishedPanel.transform.localScale = Vector3.zero;
		
		GlobalVariables.Instance.playerCar.enabled = false;

		gameFinishedPanel.transform.DOScale(1, 0.5f).SetEase(Ease.InOutQuint);
		totalDriftPoints.Text = $"Total Drifpoints: {CountingPointsDrifting.instance.totalPoints_}";
		totalDriftPoints.Play();
		cashEarned.Text = $"Driftcoins earned: {(int)(CountingPointsDrifting.instance.totalPoints_ / 20)}";

		float current_driftcoinsPlayer = SaveSystem.Instance.ReadStoreData();
		current_driftcoinsPlayer = current_driftcoinsPlayer + (CountingPointsDrifting.instance.totalPoints_ / 20);
		SaveSystem.Instance.SaveStoreData((int)current_driftcoinsPlayer);
	}

	public void ReturnMainMenu()
	{
		NetworkManager.Instance.LeaveRoom();
	}

}
