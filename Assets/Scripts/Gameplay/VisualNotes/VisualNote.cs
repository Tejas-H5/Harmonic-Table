using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualNote : MonoBehaviour {
    protected bool _finished = false;

	protected RectTransform _rtf;

	[SerializeField] protected RectTransform _noteRtf;

	public RectTransform rtf { 
		get => _rtf;
	}

	public bool finished{
		get => _finished;
	}

	float _holdStart = -2;
	float _holdEnd = -2;

	public float holdStart{
		get => _holdStart;
	}

	public float holdEnd{
		get => _holdEnd;
	}

	public float holdStartNorm{
		get => Mathf.Clamp(_holdStart/_note.duration, -1, 100);
	}

	public float holdEndNorm{
		get => Mathf.Clamp(_holdEnd/_note.duration, -1, 100);
	}

	public float holdDuration{
		get => holdEnd - holdStart;
	}

	//The note we are based on
	protected Note _note;
	//Sets this note's data to n.
	public void Bind(Note n){
		_note = n;
	}
	
	bool _isDead = false;

	public bool isDead{
		get => _isDead;
	}

	//the actual time elapsed
	protected float _lerpTime = 0;

	protected virtual void Awake(){
		_rtf = GetComponent<RectTransform>();
	}

	bool finishedPress = false;

	protected virtual void Hold(){
		if((_holdStart < -1)||finishedPress){
			_holdStart = _lerpTime;
			finishedPress = false;
		}
		_holdEnd = _lerpTime;
	}

	//I have to call it something other than Release
	protected virtual void Unhold(){
		finishedPress = true;
	}

	protected virtual void ResetNote(){
		ResetTiming();
	}

	public void ResetTiming(){
		_isDead = false;
        _lerpTime = 0;
        _holdStart = -2;
        _holdEnd = -2;
        finishedPress = false;
	}

	//lf goes from positive to zero
	public abstract void BeforeNote(float lf);

	//lf goes from 0 to 1
	public abstract void DuringNote(float lf);

	//lf goes from 0 to some positive number
	public abstract void AfterNote(float lf);

	public void Interpolate(double t){
		_lerpTime = (float)(t - _note.absoluteTime - BeatmapTiming.universalOffset);

    	if(_lerpTime < 0.0f){
			//Note hasn't happened yet
    		BeforeNote(-_lerpTime/BeatmapTiming.foresight);
    	} else if(_lerpTime < _note.duration+0.09f){
            //The note has started.
            DuringNote(_lerpTime/(_note.duration+0.09f));
    	} else {
    		float lf = 4f*(_lerpTime - (_note.duration+0.09f))/BeatmapTiming.foresight;
    		//The note has ended
    		AfterNote(lf);
    		if(lf > 1)
    			_isDead = true;
    	}
	}

	public void SetTransform(Vector3 pos, float scale) {
		_rtf.anchoredPosition = pos;
		_noteRtf.localScale = scale * Vector3.one;
		ResetNote();
	}
}
