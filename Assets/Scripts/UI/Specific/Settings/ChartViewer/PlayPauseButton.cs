using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButton : MonoBehaviour {
	public Sprite playSprite;
	public Sprite pauseSprite;

    void OnEnable(){
    	UpdatePlaybutton();
    }

    public void UpdatePlaybutton(){
        SongPlayer player = SongPlayer.instance;
        var playbuttonImage = GetComponent<Image>();
        if(player.isPaused||(!player.isPlaying)){
            playbuttonImage.sprite = playSprite;
        } else {
            playbuttonImage.sprite = pauseSprite;
        }	
    }

    public void PlayPause(){
    	SongPlayer player = SongPlayer.instance;
    	if(player.isPaused||(!player.isPlaying)){
    		player.ResumeSong();
    	} else {
    		player.PauseSong();
    	}

    	UpdatePlaybutton();
    }    
}
