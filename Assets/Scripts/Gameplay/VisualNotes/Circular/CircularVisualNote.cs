using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CircularVisualNote : VisualNote, IPressable {	
	//The size of the note
	public float scale = 100.0f;
	[SerializeField] protected HitResultImage _resultImage;

    //public abstract override void Interpolate(double absoluteTime);
    public float RecalcRadius(){
		float currentWidth = Screen.width * _noteRtf.localScale.x / _rootRtf.rect.width;
		return currentWidth/2.0f;
    }

    //called whenever its pressed
    public void Press(){
    	Hold();
    }

    public void Release(){
    	Unhold();
    }

    protected float _initScale;

    //used solely to calculate the radius
	protected RectTransform _rootRtf;

    protected override void ResetNote(){
    	base.ResetNote();

    	//reset other internal variables
		_initScale = _noteRtf.localScale.x;
    }

    void Start(){
    	_rootRtf = transform.root as RectTransform;
    	_initScale = _noteRtf.localScale.x;
    }

	protected override void Hold() {
		base.Hold();
	}
}
