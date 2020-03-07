using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationSlider : MonoBehaviour {
    Slider durationSlider;    

    void Start(){
        durationSlider = GetComponent<Slider>();
    }

    void Update(){
        SongPlayer player = SongPlayer.instance;
        
    	if(!player.isPlaying)
    		return;

    	durationSlider.value = (float)(player.songTime/player.selectedDuration);
    }
}
