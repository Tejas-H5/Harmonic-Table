using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationSlider : MonoBehaviour {
    Slider _durationSlider;    

    void Start(){
        _durationSlider = GetComponent<Slider>();
    }

    void Update(){
        SongPlayer player = SongPlayer.instance;
        
    	if(!player.isPlaying)
    		return;

    	_durationSlider.value = (float)(player.songTime/player.selectedDuration);
    }
}
