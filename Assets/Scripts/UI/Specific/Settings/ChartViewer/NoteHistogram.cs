using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteHistogram : MonoBehaviour {
	public Text lowNoteText;
	public Text hiNoteText;
    public Text maxbarText;

	public RectTransform area;

	public GameObject histBar;

	List<GameObject> _bars = new List<GameObject>();
    List<int> _hist = new List<int>();

    public void UpdateHistogram(){
        SongPlayer player = SongPlayer.instance;
        
    	Beatmap bm = player.loadedBeatmap;
    	if(bm == null){
    		return;
    	}

    	//find the lowest and highest notes from what's selected
    	float lowNote = -1;
    	float highNote = -1;
    	for(int i = 0; i < bm.numTracks; i++){
    		if(!player.IsTrackEnabled(i))
    			continue;
    		var t = bm.GetTrack(i);
    		float l = t.lowestNote;

    		if((lowNote<0)||(l < lowNote)){
    			lowNote = l;
    		}

    		float h = t.highestNote;
    		if((highNote<0)||(h > highNote)){
    			highNote = h;
    		}
    	}

        if(lowNote==-1){
            //there were no enabled tracks.
            for(int i = 0; i < _bars.Count; i++) {
                _bars[i].SetActive(false);
            }
            lowNoteText.text = "here you'll see";
            hiNoteText.text = "the keys you'll need";
            maxbarText.text = "note frequencies";
            return;
        }

    	int low = (int)lowNote;
    	int high = (int)(Mathf.Floor(highNote))+1;

        print("["+low.ToString()+","+high.ToString()+"]");

    	lowNoteText.text = Theory.NoteToString(low);
    	hiNoteText.text = Theory.NoteToString(high);

        //the real number of _bars needed
        int n = high-low+1;

        for(int i = _hist.Count; i < n; i++){
            //add more ints to our list if we don't have enough
           // print("ye boi");
            _hist.Add(0);
        }

        for(int i = 0; i < n; i++){
            _hist[i] = 0;
        }

		for(int i = 0; i < bm.numTracks; i++){
    		if(!player.IsTrackEnabled(i))
    			continue;

    		var t = bm.GetTrack(i);
    		for(int j = 0; j < t.notes.Count; j++) {
    			int note = (int)(Theory.HertzToNote(t.notes[j].frequency));
    			_hist[note-low]++;
    		}
    	}

    	int maxBar = 0;
    	for(int i = 0; i < n; i++){
    		if(_hist[i]>maxBar)
    			maxBar = _hist[i];
    	}

        maxbarText.text = maxBar.ToString();

    	float h0 = area.rect.height/ (float)maxBar;
    	float w = area.rect.width / (float)n;

    	
    	for(int i = 0; i < n; i++){
    		if(_bars.Count <= i){
    			var g = Instantiate(histBar, area) as GameObject;
    			_bars.Add(g);
    		}
            _bars[i].SetActive(true);
    		var barRtf = _bars[i].GetComponent<RectTransform>();
    		barRtf.anchoredPosition = new Vector2(w*(float)i,0);
    		barRtf.sizeDelta = new Vector2(w, h0 * (float)_hist[i]);
    	}

    	for(int i = n; i < _bars.Count; i++) {
    		_bars[i].SetActive(false);
    	}
    }
}
