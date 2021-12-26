using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
	[SerializeField] private Slider volumeSlider;

	private FMOD.Studio.Bus Master;
	private FMOD.Studio.Bus Bgm;
	private FMOD.Studio.Bus Se;
	private float bgmVolume = 0.5f;
	private float seVolume = 0.5f;
	private float masterVolume = 1f;

	private void Start()
	{
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

		volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
	}

	private void Update()
	{
		Bgm.setVolume(bgmVolume);
        Se.setVolume(seVolume);
        Master.setVolume(masterVolume);
	}

	private void OnSliderValueChanged()
	{
		bgmVolume = volumeSlider.value;
		seVolume = volumeSlider.value;
		masterVolume = volumeSlider.value;
	}
}
