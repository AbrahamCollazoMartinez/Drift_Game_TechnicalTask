using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrunoMikoski.TextJuicer;

public class SetTitleMainMenu : MonoBehaviour
{
	[SerializeField] private TMP_TextJuicer textTitle;

	public void SetTextMenu(string text)
	{
		textTitle.Text = text;
		textTitle.Play();
	}
}
