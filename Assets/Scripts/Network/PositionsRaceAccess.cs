using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

public class PositionsRaceAccess : MonoBehaviour
{
	[SerializeField] private PositionsRace[] positionsRace;

	public static PositionsRaceAccess instance;

	private void Awake()
	{
		instance = this;
	}

	public Transform GetPositionAvaliable()
	{

		if (PhotonNetwork.IsMasterClient)
		{
			return positionsRace[0].position;
		}
		else
		{
			return positionsRace[1].position;

		}



	}
}

[Serializable]
class PositionsRace
{
	public Transform position;
}