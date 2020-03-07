using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapTrack{
	List<Note> _notes;
	public List<Note> notes{
		get => _notes;
	}

	public string trackName = "";

	//probably not needed
	public string instrumentName = "";

	//The position of the cursor in notes.
	int position = 0;

	public BeatmapTrack(){
		_notes = new List<Note>();
	}

	public float Speed(){
		if(_notes.Count==0)
			return -1.0f;
		return _notes.Count/(float)duration;
	}

	//doesn't yet take the keyboard layout into consideration
	public float Difficulty(){
		float res = 0.0f;

		double lt = _notes[0].absoluteTime;
		float ln = Theory.HertzToNote(_notes[0].frequency);

		float segments = 1;
		float chord = 1;
		for(int i = 1;i < _notes.Count; i++){
			float ni = Theory.HertzToNote(_notes[i].frequency);
			float dn = Mathf.Abs(ln - ni);
			ln = ni;

			float dt = (float)(_notes[i].absoluteTime-lt);
			lt = _notes[i].absoluteTime;

			if(dt<0.000000001f){
				chord++;
				continue;
			}
			//the change in frequency over the change in time
			//also counts chords as harder
			res += (dn*chord)/dt;
			chord=1f;
			segments++;
		}

		return res/segments;
	}

	public double startTime{
		get{
			if(_notes.Count <= 0)
				return -1.0;		
			return _notes[0].absoluteTime;
		}
	}

	public double endTime{
		get{
			if(_notes.Count <= 0)
				return -1.0;		
			return _notes[_notes.Count-1].absoluteTime + (double)_notes[_notes.Count-1].duration;
		}
	}

	public double duration {
		get {
			return endTime - startTime;
		}
	}

	public float lowestNote{
		get{
			if(_notes.Count <= 0)
				return 10000000;
			float min = _notes[0].frequency;

			foreach(Note n in _notes){
				if(n.frequency < min)
					min = n.frequency;
			}
			return Theory.HertzToNote(min);
		}
	}

	public float highestNote{
		get{
			if(_notes.Count <= 0)
				return -1;
			float max = _notes[0].frequency;

			foreach(Note n in _notes){
				if(n.frequency > max)
					max = n.frequency;
			}
			return Theory.HertzToNote(max);
		}
	}

	public void ResetPosition(){
		position = 0;
	}

	public Note NextNote(){
		if(position >= _notes.Count)
			return null;

		var note = _notes[position];
		position++;
		return note;
	}
}
