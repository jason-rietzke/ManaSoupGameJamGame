using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
	public Slider volumeSlider;

    FMOD.Studio.EventInstance seTestEvent;
    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Bgm;
    FMOD.Studio.Bus Se;
    float bgmVolume = 0.5f;
    float seVolume = 0.5f;
    float masterVolume = 1f;

	void Start()
	{
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

		volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
	}

	void Update()
	{
		Bgm.setVolume(bgmVolume);
        Se.setVolume(seVolume);
        Master.setVolume(masterVolume);
	}

	public void OnSliderValueChanged()
	{
		bgmVolume = volumeSlider.value;
		seVolume = volumeSlider.value;
		masterVolume = volumeSlider.value;
	}
}
