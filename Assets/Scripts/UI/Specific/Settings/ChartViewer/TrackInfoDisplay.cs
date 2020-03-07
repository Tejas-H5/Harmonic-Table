using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackInfoDisplay : MonoBehaviour {
	public Text npmText;
	public Text diffText;
	public Text durText;

    public void UpdateTrackInfo(){
    	SongPlayer player = SongPlayer.instance;
    	var bm = player.loadedBeatmap;

		float diff = 0;
		for(int i = 0; i < bm.numTracks; i++){
    		if(!player.IsTrackEnabled(i))
    			continue;

    		float res = bm.GetTrack(i).Difficulty();
    		
    		if(res < 0) {
    			diff = res;
    			break;
    		} else {
    			diff += res;
    		}
    	}

    	if(diff<0){
	    	diffText.text = "impossible, need more keys";
	    	diffText.color = Color.red;
		} else{
			diffText.text = diff.ToString("0.0") + " difficulty";
			diffText.color = Color.white;
		}

		float nps = 0;
    	for(int i = 0; i < bm.numTracks; i++){
    		if(!player.IsTrackEnabled(i))
    			continue;

    		nps += (float)bm.GetTrack(i).Speed();
    	}

		npmText.text = (nps*60f).ToString("0.0") + "notes/mim";

		durText.text = Theory.DurationToString(player.selectedDuration);
    }
}
