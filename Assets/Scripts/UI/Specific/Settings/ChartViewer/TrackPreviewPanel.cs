using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackPreviewPanel : MonoBehaviour {
	//these two need to be initialized first
	public TrackListGet trackListGet;
	public int trackIndex;

    [SerializeField] Text trackNameTextbox;
    [SerializeField] Text instrumentNameTextbox;
    [SerializeField] Text speedTextbox;
    [SerializeField] Text timeTextbox;
    [SerializeField] UIBoolToggle isEnabledToggle;

    //should be called through the isEnabledToggle
    public void SetTrack(bool v){
		trackListGet.SetPlaying(trackIndex, v);
    }

    public bool isEnabled{
    	get => isEnabledToggle.val;
    	set{
    		isEnabledToggle.SetValue(value);
    	}
    }

	string _trackName;
	public string trackName{
		get => _trackName;
		set{
			_trackName = value;
			trackNameTextbox.text = value;
		}
	}

	string _instrumentName;
	public string instrumentName{
		get => _instrumentName;
		set{
			_instrumentName = value;
			instrumentNameTextbox.text = value;
		}
	}

	string _speed;
	public string speed{
		get => _speed;
		set{
			_speed = value;
			speedTextbox.text = value;
		}
	}

	string _time;
	public string time{
		get => _time;
		set{
			_time = value;
			timeTextbox.text = value;
		}
	}

}
