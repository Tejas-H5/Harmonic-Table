using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DspFpsCounter : MonoBehaviour {
	public SongPlayer player;
	Text _textReadout;

	void Start(){
		_textReadout = GetComponent<Text>();
	}

	float[] _frames = new float[3];
    int _currentFrame = 0;

	public Color best = new Color(0.0f, 1.0f, 1.0f);
	public Color good = new Color(0.0f, 1.0f, 0);
	public Color ok = new Color(1.0f, 1.0f, 0);
	public Color bad = new Color(1.0f,0,0);

    void Update(){
    	_frames[_currentFrame] = 1.0f/(float)player.dspDelta;
        _currentFrame = (_currentFrame+1)%_frames.Length;

    	float calculated = (_frames[0]+_frames[1]+_frames[2]) / 3.0f;
    	_textReadout.text = calculated.ToString("0.00") + " DSPfps";

    	if(calculated > 60){
    		_textReadout.color = best;
    	} else if(calculated > 20){
    		_textReadout.color = Color.Lerp(good, best, (calculated-20)/20.0f);
    	} else if (calculated > 20){
    		_textReadout.color = Color.Lerp(ok, good, (calculated-20)/20.0f);
    	} else {
    		_textReadout.color = Color.Lerp(bad, ok, calculated/20.0f);
    	}
    }
}
