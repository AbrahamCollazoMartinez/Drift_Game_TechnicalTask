using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	private static NetworkManager _instance;
	public static NetworkManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<NetworkManager>();
				if (_instance == null)
				{
					GameObject obj = new GameObject();
					obj.name = typeof(NetworkManager).Name;
					_instance = obj.AddComponent<NetworkManager>();
				}
			}
			return _instance;
		}
	}

	public virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as NetworkManager;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		LoadingScreenManager.Instance.showLoading(true);
		if (!PhotonNetwork.IsConnected)
			StartCoroutine(Autoconnect());
	}



	IEnumerator Autoconnect()
	{
		yield return new WaitForSeconds(1);

		LoadingScreenManager.Instance.showLoading(false);
		MainMenuManager.instance.ShowMainMenu();

		PhotonNetwork.LocalPlayer.NickName = NameGeneratorManager.playerName;
		PhotonNetwork.ConnectUsingSettings();


	}

	public void JoinOrCreateRoom()
	{
		LoadingScreenManager.Instance.showLoadingScreen(true);
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2; // Set your desired max players count

		PhotonNetwork.JoinOrCreateRoom("RaceRoom", roomOptions, TypedLobby.Default);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		PlayersRoomDisplay.Instance?.SetPlayerCount(PhotonNetwork.CurrentRoom.PlayerCount);

	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		PlayersRoomDisplay.Instance?.SetPlayerCount(PhotonNetwork.CurrentRoom.PlayerCount);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);

		PlayersRoomDisplay.Instance?.SetPlayerCount(PhotonNetwork.CurrentRoom.PlayerCount);


	}

	public void StartGame()
	{
		if (PhotonNetwork.IsMasterClient)
			PhotonNetwork.LoadLevel(1);

	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarning("Disconnected from Photon: " + cause.ToString());
	}
	[PunRPC]
	public void StartCountdownStartMatch()
	{
		Debug.Log("counter started");
		StartCoroutine(PlayersRoomDisplay.Instance.StartMatch());
	}

	public void LeaveRoom()
	{
		if (PhotonNetwork.InRoom)
		{
			// Leave the current room
			PhotonNetwork.LeaveRoom();
		}
		else
		{
			Debug.LogWarning("Not currently in a room.");
		}
	}

	public override void OnLeftRoom()
	{
		Debug.Log("Left the room");

		// Load the menu scene (replace "MenuScene" with the actual name of your menu scene)
		SceneManager.LoadScene(0);
	}

}
