using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BrunoMikoski.TextJuicer;
using System;

public class CounterDisplay : MonoBehaviour
{
	[SerializeField] private TMP_TextJuicer textAnimation;

	public void SetText(string text)
	{

		//textCounter.text = text;
		textAnimation.Text = text;
		textAnimation.Play();
	}

	public void CleanText()
	{
		textAnimation.Text = string.Empty;

	}

}

