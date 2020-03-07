using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyScript : MonoBehaviour, IPressable, IComparable<KeyScript>{
	public Color pressedCol;
	public Color normalCol;

	public Text noteLetter;
	public Text noteNumber;
	public Image keyImage;

	//the keyboard to send the frequency to
	public KeyboardScript keyboard;

	RectTransform _parentRtf;

	RectTransform _rtf;

	public RectTransform rtf { 
		get {
			return _rtf;
		}
	}
	
	Oscilator _osc;

	public void Settings(OscilatorSettings s){
		_osc.settings = s;
	}

	public void SetFrequency(float note){
		frequency = note;
		string name = Theory.NoteLetter(note);
		string number = Theory.NoteNumber(note).ToString();
		noteLetter.text = name;
		noteNumber.text = number;
	}

	public void SetColor(Color c){
		normalCol = c;
		keyImage.color = normalCol;
	}

	public void SetPressedColor(Color c){
		pressedCol = c;
	}

	public float nextSignal{
		get => _osc.GetNext();
	}

	public bool resonating{
		get =>  _osc.activated;
	}

	//the frequency of the note is stored with the oscilator
	public float frequency{
		get => _osc.frequency;
		set{
			_osc.frequency = value;
		}
	}

	public int CompareTo(KeyScript other){
		return frequency.CompareTo(other.frequency);
	}

	protected  void Awake(){
		_osc = new Oscilator();
		_rtf = GetComponent<RectTransform>();
		_parentRtf = transform.parent.GetComponent<RectTransform>();		
	}

	//the keyboard calls this function on the key once it is sent to the keyboard
	//ye i know bad design but idk how else to do what im doing
	public void Resonate(){
		_osc.Press();
	}

	public void StopResonating(){
		_osc.Reset();
	}

	public void Press(){
		keyboard.Press(this);
	}

	public void Release(){
		//idk lol
	}

	public float RecalcRadius(){
		float currentWidth = Screen.width * _rtf.rect.width / _parentRtf.rect.width;
		return 0.5f * currentWidth * keyboard.chordSize;
	}

	public void Update(){
		_osc.Advance();
		keyImage.color = Color.Lerp(normalCol, pressedCol, _osc.gain);
		_rtf.localScale = Mathf.Lerp(1, 0.9f, _osc.gain) * Vector3.one;
	}
}
