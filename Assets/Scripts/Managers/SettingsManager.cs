using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private TMP_Dropdown resolutionDropdown;
	[SerializeField] private Toggle fullscreenToggle;
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private TMP_Text percentageVolumeText;


	private Resolution[] resolutions;

	void Start()
	{
		// Initialize settings on start
		InitializeSettings();
	}

	void InitializeSettings()
	{
		// Set up resolution dropdown
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();

		foreach (Resolution resolution in resolutions)
		{
			resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
		}

		// Set the current resolution
		resolutionDropdown.value = GetCurrentResolutionIndex();
		resolutionDropdown.RefreshShownValue();

		// Set fullscreen toggle
		fullscreenToggle.isOn = Screen.fullScreen;

		// Set volume slider
		float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); // Default to full volume
		volumeSlider.value = savedVolume;
		SetVolume(savedVolume);
	}

	public void SetVolume(float volume)
	{
		// Set the volume using the Audio Mixer
		audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
		PlayerPrefs.SetFloat("Volume", volume); // Save volume preference


		int percentage = Mathf.RoundToInt(volume * 100);
		percentageVolumeText.text = $"{percentage}%";

	}

	public void SetFullscreen(bool isFullscreen)
	{
		// Set fullscreen mode
		Screen.fullScreen = isFullscreen;
	}

	public void SetResolution(int resolutionIndex)
	{
		// Set the resolution based on the selected index
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	private int GetCurrentResolutionIndex()
	{
		// Find the index of the current screen resolution
		Resolution currentResolution = Screen.currentResolution;

		for (int i = 0; i < resolutions.Length; i++)
		{
			if (resolutions[i].width == currentResolution.width && resolutions[i].height == currentResolution.height)
			{
				return i;
			}
		}

		// If the current resolution is not in the available resolutions, return 0
		return 0;
	}
}
