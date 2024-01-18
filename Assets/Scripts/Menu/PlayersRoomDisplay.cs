using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrunoMikoski.TextJuicer;
using System;

public class PlayersRoomDisplay : MonoBehaviour
{
	[SerializeField] private TMP_TextJuicer textInfo;
	[SerializeField] private Timer timerAccess;

	public static PlayersRoomDisplay Instance;
	private void Awake()
	{
		Instance = this;
		this.gameObject.SetActive(false);
	}

	public void SetPlayerCount(int countPlayers)
	{
		this.gameObject.SetActive(true);
		timerAccess.enabled = false;

		if (countPlayers < 2)
		{
			textInfo.Text = $"Players needed {countPlayers}/{2}";
			textInfo.Play();
		}
		else if (countPlayers == 2)
		{
			textInfo.Text = $"Get Ready!";
			textInfo.Play();
			NetworkManager.Instance.StartCountdownStartMatch();
		}
	}

	public void OnTimeChange(string time)
	{
		textInfo.Text = time.ToString();
		textInfo.Play();
	}

	public IEnumerator StartMatch()
	{
		yield return new WaitForSeconds(1);
		timerAccess.enabled = true;
		timerAccess.StartTimer();
	}

	public void OnLoadingMatch()
	{
		this.gameObject.SetActive(false);
		NetworkManager.Instance.StartGame();

	}
}
