using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lexic;
using BrunoMikoski.TextJuicer;

public class NameGeneratorManager : MonoBehaviour
{
	[SerializeField] private NameGenerator nameGenerator;
	[SerializeField] private TMP_TextJuicer nameDisplay;

	public static string playerName;
	private void Start()
	{
		if (PlayerPrefs.HasKey("PlayerName"))
		{
			playerName = PlayerPrefs.GetString("PlayerName");
		}
		else
		{
			playerName = nameGenerator.GetNextRandomName();
			PlayerPrefs.SetString("PlayerName", playerName);
		}

		nameDisplay.Text = playerName;
		nameDisplay.Play();
	}

}
