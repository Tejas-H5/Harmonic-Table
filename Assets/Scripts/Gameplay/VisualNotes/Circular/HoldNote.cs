using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//mustn't inherit from IPressable
public class HoldNote : CircularVisualNote {
	public Transform approachCircle;
	public Image durationCircle;
	public DialScript pressDurationCircle;

	protected override void Hold(){
		base.Hold();
		if(isDead)
			return;

		//don't animate if it isn't time
		if((_lerpTime < 0)||(holdEndNorm > 1))
			return;

        //animate note holding
		pressDurationCircle.SetFill(holdDuration/_note.duration);
        _noteRtf.localScale = _initScale * Vector3.one * Mathf.Lerp(1,0.9f,holdDuration/_note.duration);
	}

    public override void BeforeNote(float lf){
        //animate scale and approachCircle
        //scaleAmount goes from 0.5 to 1 if lf goes from 1 to 0
        float scaleAmount = (1f - lf/2);
        _noteRtf.localScale = scaleAmount * _initScale * Vector3.one;
        approachCircle.localScale = (1.0f + (lf *  BeatmapTiming.approachScale)*_initScale/_noteRtf.localScale.x) * Vector3.one;

        //Set the note's initial states
        durationCircle.fillAmount = 0.0f;
        pressDurationCircle.SetFill(0.0f);
        _firedAnimation = false;
    }

    public override void DuringNote(float lf){
        //The note has started. set the note's during states
        _noteRtf.localScale = _initScale * Vector3.one;

        //Animate the duration circle
        durationCircle.fillAmount = lf;
    }


    bool _firedAnimation = false;

    public override void AfterNote(float lf){
        //The note has ended. set the end states
        durationCircle.fillAmount = 1.0f;

        //animate fading away
        _noteRtf.localScale = Mathf.Lerp(1.0f, 0.0f, lf) * _initScale * 0.9f * Vector3.one;

        if(!_firedAnimation){
        	_firedAnimation = true;
        	_resultImage.Animate(BeatmapTiming.GetTiming(holdStartNorm, holdEndNorm));
        }
    }

    protected override void ResetNote(){
    	base.ResetNote();

    	//reset UI
		durationCircle.fillAmount = 0;
		pressDurationCircle.SetFill(0);

		approachCircle.localScale = BeatmapTiming.approachScale * Vector3.one;
    }
}
