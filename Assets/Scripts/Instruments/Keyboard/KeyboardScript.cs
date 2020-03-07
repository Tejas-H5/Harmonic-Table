using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class KeyboardScript : Instrument {
	public GameObject hexNote;
	OscilatorSettings _thisSettings;
	public GameObject _settingsUIPanel;
	public RectTransform keyCanvas;

	//physical parameters
	public int numRows = 6;
	public int numCols = 8;
	public bool temporal = false;

	//cosmetic parameters
	public Color normalColor;
	public Color sharpColor;
	public Color pressedColor;
	public bool touchCircles = false;

	//for when we get notes to 'Press'
	public float tolerance = 1.0f;

	public void SetTouchCircleUse(bool b){
		touchCircles = b;
	}

	// a list of all keys
	List<KeyScript> _keys;
	// the rect transform of this ui element.
	RectTransform _rtf;
	public float width {get {return _rtf.rect.width;}}
	public float height {get {return _rtf.rect.height;}}
	float _noteWidth;

	//The oscilators that we have held down
	List<KeyScript> _heldKeys;

	//used to determine the maximum number of rows and columns.
	int _maxRowCols = 40;
	
	//The root note offset parameters
	public float rootNoteLetter = 0.0f;
	public float rootNoteNumber = 4.0f;
	public float upwardsDelta = 3.0f;
	public bool upwardsNegate = true;
	public float upwardsDistort = 0.0f;
	public float downwardsDelta = 4.0f;
	public bool downwardsNegate = false;
	public float downwardsDistort = 0.0f;

	public void SetRootNumber(float f){
		rootNoteNumber = f;
		RescaleNotes();
	}
	public void SetRootLetter(float f){
		rootNoteLetter = f % 12.0f;
		print(f.ToString()+"->"+rootNoteLetter.ToString());
		RescaleNotes();
	}
	public void SetUpwardsDelta(float f){
		upwardsDelta = Mathf.Abs(f);
		RescaleNotes();
	}
	public void SetDownwardsDelta(float f){
		downwardsDelta = Mathf.Abs(f);
		RescaleNotes();
	}
	public void SetUpwardsNegate(bool b){
		upwardsNegate = b;
		RescaleNotes();
	}
	public void SetDownwardsNegate(bool b){
		downwardsNegate = b;
		RescaleNotes();
	}
	public void SetUpwardsDistort(float f){
		upwardsDistort = Mathf.Clamp01(f);
		RescaleNotes();
	}
	public void SetDownwardsDistort(float f){
		downwardsDistort = Mathf.Clamp01(f);
		RescaleNotes();
	}

	public float majorOffset{
		get{
			float f = upwardsDelta + upwardsDistort;
			return (upwardsNegate ? -f  : f);
		}
	}

	public float minorOffset{
		get{
			float f = downwardsDelta + downwardsDistort;
			return (downwardsNegate ? -f  : f);
		}
	}

	KeyScript MakeNote(){
		var note = Instantiate(hexNote, keyCanvas).GetComponent<KeyScript>();
		note.keyboard = this;
		note.Settings(_thisSettings);
		return note;
	}

	void DeleteNote(int n){
		var note = _keys[n];
		_keys.RemoveAt(n);
		Destroy(note.gameObject);
	}

	void InitNotes(){
		RescaleNotes();
	}

	void RepositionNotes(){
		float noteHeight = height/numRows;
		noteHeight = (height+noteHeight/2)/numRows;

		_noteWidth = 2.0f*noteHeight/Mathf.Sqrt(3);

		var offset = new Vector2(
				-(3.0f*_noteWidth/4.0f)*numCols*0.5f + _noteWidth*0.5f, 
				-height*0.5f
			);

		for(int i = 0; i < numCols; i++){
			for(int j = 0; j < numRows; j++){
				var note = _keys[i+numCols*j];

				note.rtf.anchoredPosition = new Vector2(
					i*(3.0f*_noteWidth/4.0f), 
					j*noteHeight + (i%2==0 ? noteHeight/2 : 0)
				) + offset;

				note.rtf.sizeDelta = new Vector2(_noteWidth, noteHeight);
			}
		}
	}

	void ColorNotes(){
		for(int i = 0; i < _keys.Count; i++){
			var note = _keys[i];
		if(Theory.IsSharp(Theory.HertzToNote(note.frequency))){
				note.SetColor(sharpColor);
			} else {
				note.SetColor(normalColor);
			}
			note.SetPressedColor(pressedColor);
		}
	}

	//Minor third -> 3 semitione diff
	//Major third -> 4 semitione diff
	//Perfect Fifth -> 7 semitone diff
	void RescaleNotes(){
		RepositionNotes();

		float rootNote = Theory.LetterNumberNote(rootNoteLetter, rootNoteNumber);
		print("root note: " + rootNote.ToString());
		float columnNote = rootNote;

		for(int i = 0; i < numCols; i++){
			float rowNote = columnNote;
			for(int j = 0; j < numRows; j++){
				var note = _keys[i+numCols*j];
				note.SetFrequency(rowNote);
				note.frequency = Theory.NoteToHertz(rowNote);
				rowNote += majorOffset-minorOffset;
			}

			//Alternate stepping down and up
			if(i%2==0){
				//default -3
				columnNote += minorOffset;
			} else {
				//default +4
				columnNote += majorOffset;
			}
		}

		//sort the keys based on frequency
		_keys.Sort();
		print("Sorted");

		ColorNotes();
	}

	void AddNotes(int n){
		ReleaseAllNotes();
		for(int i = 0; i < n; i++){
			_keys.Add(MakeNote());
		}
	}

	void RemoveNotes(int n){
		ReleaseAllNotes();
		for(int i = 0; i < n; i++){
			DeleteNote(_keys.Count-1);
			if(_keys.Count==0)
				break;
		}
	}

	void AddRow(){
		AddNotes(numCols);
		numRows++;
		InitNotes();
	}

	void RemoveRow(){
		RemoveNotes(numCols);
		numRows--;
		InitNotes();
	}

	void AddColumn(){
		AddNotes(numRows);
		numCols++;
		InitNotes();
	}

	void RemoveColumn(){
		RemoveNotes(numRows);
		numCols--;
		InitNotes();
	}

	public void SetRows(float f){
		int n = Theory.RoundFloat(f);
		if(n < numRows){
			for(int i = 0; i < (numRows - n);i++){
				RemoveRow();
				if(numRows <= 1)
					break;
			}
		} else if (n > numRows){
			for(int i = 0; i < (n - numRows);i++){
				AddRow();
				if(numRows >= _maxRowCols)
					break;
			}
		}
	}

	public void SetCols(float f){
		int n = Theory.RoundFloat(f);
		if(n < numCols){
			for(int i = 0; i < (numCols - n);i++){
				RemoveColumn();
				if(numCols <= 1)
					break;
			}
		} else if (n > numCols){
			for(int i = 0; i < (n - numCols);i++){
				AddColumn();
				if(numCols >= _maxRowCols)
					break;
			}
		}
	}

	//--------------Actual Behaviours--------------------------------------------------------------------

	void Start(){
		_rtf = GetComponent<RectTransform>();
		_thisSettings = GetComponent<OscilatorSettings>();

		_keys = new List<KeyScript>();
		_keys.Capacity = numRows * numCols;
		//a buffer to store the held oscilators
		_heldKeys = new List<KeyScript>();
		//32 is arbitrary.
		_heldKeys.Capacity = 32;

		AddNotes(numRows*numCols);
		InitNotes();
	}

	public override bool CanPress(Note n){
		for(int i = 0; i < _keys.Count; i++){
			if(Mathf.Abs(_keys[i].frequency-n.frequency) < tolerance){
				return true;
			}
		}

		return false;
	}

	public bool extravagentDisplay = true;

	//get all notes of a certain frequency.
	void Find(float frequency, out int start, out int end) {
		int a = 0;
		int n = _keys.Count;		
		for(int jump = n/2; jump >= 1; jump/=2) {
			while((a+jump)<n && (_keys[a+jump].frequency < (frequency-tolerance))) {
				a += jump;
			}
		}
		
		if(a + 1 >= n){
			//the frequency isn't in the list
			start = 0;
			end = 0;
			return;
		}
		
		a++;
		start = a;

		while(a < n){
			if(_keys[a].frequency > (frequency + tolerance)) {
				break;
			}
			a++;
		}
		
		end = a;
	}

	public override void Press(Note n){
		/*
		for(int i = 0; i < _keys.Count; i++){
			var k = _keys[i];
			if(Mathf.Abs(k.frequency-n.frequency) < tolerance){
				Press(k);
				//This means that we only press 1 of any key with the frequency
				if(!extravagentDisplay)
					break;
			}
		}*/
		int start, end;
		Find(n.frequency, out start, out end);

		if(start==end){
			print("note not found: " + Theory.NoteToString(Theory.HertzToNote(n.frequency)));
		}

		for(int i = start; i < end; i++){
			Press(_keys[i]);
			//This means that we only press 1 of any key with the frequency
			if(!extravagentDisplay)
				break;
		}
	}

	public override void CreateVisualNote(Note n){
		int start, end;
		Find(n.frequency, out start, out end);

		for(int i = start; i < end; i++){
			var k = _keys[i];
			SpawnNote(n, k.rtf.anchoredPosition, k.rtf.rect.width);
			
			//This means that we only press 1 of any key with the frequency
			if(!extravagentDisplay)
					break;
		}
	}

	//adds the key's oscilator to the oscilator stack if it isn't already there
	public void Press(KeyScript key){
		key.Resonate();
		if(_heldKeys.IndexOf(key) < 0){
			_heldKeys.Add(key);
		}
	}

	public override void ReleaseAllNotes(){
		foreach(var k in _heldKeys){
			k.StopResonating();
		}
		_heldKeys.Clear();
		//Clear the scheduled input
	}

	void OnAudioFilterRead(float[] data, int channels){

		for(int i = 0; i < data.Length; i+=channels){
			data[i] = 0.0f;    

            int n = _heldKeys.Count;
			//add all held oscilators to the sample
			for(int j = 0; j < n; j++){
				var key = _heldKeys[j];

				//Add each oscilator to the waveform
				float noteSample = key.nextSignal;

				if(temporal){
					//TODO: weight the sample based on it's distance from the mouse, which is stored in another array
				}

				data[i] += noteSample;
			}

			data[i] = Mathf.Clamp(data[i],-1.0f,1.0f);

			//copy sample to other channels
			for(int j = 1; j < channels; j++){
				data[i+j] = data[i];
			}
		}

        CleanInput();
	}

	void RemoveInput(int i){
		_heldKeys[i] = _heldKeys[_heldKeys.Count-1];
		_heldKeys.RemoveAt(_heldKeys.Count-1);
	}

	void CleanInput(){
        int n = _heldKeys.Count; 
		for(int i = 0; i < n; i++){
		    if(!_heldKeys[i].resonating){
				RemoveInput(i);
				i--;
                n--;
			}
		}
	}

	void Update(){		
		//pause menu on - off
		if(Input.GetButtonDown("Cancel")){
			_settingsUIPanel.SetActive(!_settingsUIPanel.activeSelf);
		}
	}
}