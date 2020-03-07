using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatmapViewer : MonoBehaviour {
    public SongPlayer player;
    public GameObject noteImagePrefab;
    public float noteScale = 100;

    float playbackTime = 0.0f;

    public void SetTime(float t){
    	playbackTime = t * (float)player.loadedBeatmap.duration;
    	var rtf = (RectTransform)transform;
    	Vector2 pos = rtf.anchoredPosition;
    	rtf.anchoredPosition = new Vector2(pos.x,-playbackTime * noteScale);
    }

    //x between 0 and 1
    public void SetHorizontal(float x){
    	var rtf = (RectTransform)transform;
    	float w = rtf.rect.width;
    	rtf.anchoredPosition = new Vector2(84.0f*(x-0.5f)*(w/88.0f), rtf.anchoredPosition.y);
    }

    List<MidiNoteImage> _notes = new List<MidiNoteImage>();

    public int trackNum;

    public void PositionNotes(){
    	var trk = player.loadedBeatmap.GetTrack(trackNum);
    	float t = 0;
    	int i = 0;

    	var rtf = (RectTransform)transform;
    	float h = rtf.rect.height;
    	float w = rtf.rect.width;

    	foreach(Note n in trk.notes){
    		MidiNoteImage im;

    		if(i < _notes.Count){
    			im = _notes[i].GetComponent<MidiNoteImage>();
    		} else {
    			im = Instantiate(noteImagePrefab, transform).GetComponent<MidiNoteImage>();
    			_notes.Add(im);
    		}	
    		i++;

    		t += n.deltaTime;
    		//float y = t * noteScale;
    		float y = (float)n.absoluteTime * noteScale;
	    	float x = (w/88.0f) * (Theory.HertzToNote(n.frequency)-21);

    		im.rt.anchoredPosition = new Vector2(x, y);
    		im.rt.sizeDelta = new Vector2(w/88.0f,n.duration * noteScale);
    	}
    }
}
