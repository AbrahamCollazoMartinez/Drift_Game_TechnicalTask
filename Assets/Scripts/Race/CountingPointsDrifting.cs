using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using BrunoMikoski.TextJuicer;
using Unity.VisualScripting;

public class CountingPointsDrifting : MonoBehaviour
{

	[SerializeField] private TMP_TextJuicer driftingPoints, totalPoints;

	private float total_points = 0.0f;
	private float acumulated_points = 0.0f;
	private bool isAccumulating = false;
	private float accumulationAmount = 200f; // Set the desired accumulation amount
	private float updateInterval = 0.3f; // in seconds

	public float totalPoints_ { get { return total_points; } }
	public static CountingPointsDrifting instance;
	private void Awake()
	{
		instance = this;
	}

	private void onPlayerDriftStateChange(bool state)
	{
		if (state)
		{
			StartAccumulation();
		}
		else
		{
			StopAccumulation();
		}
	}

	private void OnEnable()
	{
		StartCoroutine(GetPlayerAtRace());
	}

	IEnumerator GetPlayerAtRace()
	{
		yield return new WaitForSeconds(1);
		CarBrain[] objectsWithScript = FindObjectsOfType<CarBrain>();
		foreach (var item in objectsWithScript)
		{
			PhotonView accesPhotonView = item.GetComponent<PhotonView>();

			if (accesPhotonView.IsMine)
			{
				GlobalVariables.Instance.playerCar = item;
			}
		}

		GlobalVariables.Instance.playerCar.onDriftStateChange += onPlayerDriftStateChange;
	}

	private void OnDisable()
	{
		GlobalVariables.Instance.playerCar.onDriftStateChange -= onPlayerDriftStateChange;
	}

	public void StartAccumulation()
	{
		isAccumulating = true;
		acumulated_points = 0;
		InvokeRepeating(nameof(AccumulatePoints), 0f, updateInterval);
	}

	public void StopAccumulation()
	{
		isAccumulating = false;
		CancelInvoke(nameof(AccumulatePoints));
		total_points += acumulated_points;
		SetDriftingPoins(0);
		SetTotalPoins((int)total_points);
	}

	private void AccumulatePoints()
	{
		acumulated_points += accumulationAmount;
		SetDriftingPoins((int)acumulated_points);
	}

	private void SetDriftingPoins(int amount)
	{
		driftingPoints.Text = $"Drifting points: {amount}";

	}

	private void SetTotalPoins(int amount)
	{
		totalPoints.Text = $"Total points: {amount}";
		totalPoints.Play();
	}

}
