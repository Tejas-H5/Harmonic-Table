using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaybackSettingsScript : MonoBehaviour {
	public SongPlayer player;

	public Slider speedSlider;
	public Text speedText;

	public Slider atSlider;
	public Text atText;

	public UIBoolToggle autoplayToggle;

	public UIBoolToggle redundantToggle;

	public UIBoolToggle transposeToggle;

	void OnEnable(){
		speedSlider.value = (float)player.playSpeed * 10.0f;
		atSlider.value = (float)BeatmapTiming.foresight * 100f;
		autoplayToggle.val = player.autoplay;
	}


	public void SetApproachTime(float s){
		BeatmapTiming.foresight = s / 100f;
		atText.text = (s/100f).ToString() + " s";
	}

	//f from 5 to 30
	public void SetPlaySpeed(float f){
		player.SetSpeed(f/10.0f);
		speedText.text = (f/10.0f).ToString("0.0") + "x";
	}
}
