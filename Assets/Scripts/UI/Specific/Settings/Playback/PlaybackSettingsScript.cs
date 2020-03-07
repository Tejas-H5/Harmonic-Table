using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaybackSettingsScript : MonoBehaviour {
	public SongPlayer player;

	public UIScrollbarInput speedSlider;
	public Text speedText;

	public UIScrollbarInput atSlider;
	public Text atText;

	public UIBoolToggle autoplayToggle;

	public UIBoolToggle redundantToggle;

	public UIBoolToggle transposeToggle;

	void OnEnable(){
		speedSlider.Value = (float)player.playSpeed;
		atSlider.Value = (float)BeatmapTiming.foresight;
		autoplayToggle.val = player.autoplay;
	}


	public void SetApproachTime(){
        float s = atSlider.Value;
		BeatmapTiming.foresight = s;
		atText.text = s.ToString() + " s";
	}

	//f from 5 to 30
	public void SetPlaySpeed(){
        float f = speedSlider.Value;
		player.SetSpeed(f);
		speedText.text = f.ToString("0.0") + "x";
	}
}
