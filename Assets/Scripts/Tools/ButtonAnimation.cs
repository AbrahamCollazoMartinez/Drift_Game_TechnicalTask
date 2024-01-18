using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
	public float punchDuration = 0.2f; // Duration of the punch animation
	public float punchScale = 1.2f; // Scale factor during the punch

	private Vector3 originalScale;
	private Button button;
	private bool isAnimating = false;

	private void Start()
	{
		button = GetComponent<Button>();
		originalScale = transform.localScale;

		// Attach the onClick listener to trigger the punch animation when the button is clicked
		if (button != null )
			button.onClick.AddListener(OnClick);
			
	}

	private void OnClick()
	{
		if (!isAnimating)
		{
			isAnimating = true;
			// Perform the punch animation
			Vector3 targetScale = originalScale * punchScale;
			transform.DOScale(targetScale, punchDuration).SetEase(Ease.OutCubic).OnComplete(OnPunchComplete);
		}
	}

	private void OnPunchComplete()
	{
		// Reset the scale to its original value after the punch animation is completed
		transform.DOScale(originalScale, punchDuration).SetEase(Ease.OutBack).OnComplete(() => isAnimating = false);
	}
}
