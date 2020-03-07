using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOffsetUIPanel : MonoBehaviour {
	[SerializeField] Slider slider;
	[SerializeField] UIUpDown updown;

	void OnEnable(){
		updown.val = (int)BeatmapTiming.universalOffset;
		slider.value = BeatmapTiming.universalOffset;
	}

	public void SetUniversalOffset(float f){
		BeatmapTiming.universalOffset = f/100.0f;
		updown.val = (int)f;
		slider.value = (float)((int)f);
	}
}
