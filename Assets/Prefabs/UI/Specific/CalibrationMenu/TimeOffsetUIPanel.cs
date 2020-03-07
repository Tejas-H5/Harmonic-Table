using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOffsetUIPanel : MonoBehaviour {
	[SerializeField] UIScrollbarInput slider;
	[SerializeField] UIUpDown updown;

	void OnEnable(){
		updown.val = (int)BeatmapTiming.universalOffset;
		slider.Value = BeatmapTiming.universalOffset;
	}

    //These two used to be one function before Unity's UnityEvent<T> class dynamic T stopped working

    public void SetUniversalOffsetSlider(){
        float f = slider.Value;
        updown.val = (int)f;
        BeatmapTiming.universalOffset = f/100.0f;
    }

	public void SetUniversalOffsetUIUpDown(){
		float f = updown.val;
        slider.SetValuePassive(f);
        BeatmapTiming.universalOffset = f/100.0f;
	}
}
