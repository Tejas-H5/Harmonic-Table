using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DspFpsCounter : MonoBehaviour {
	public SongPlayer player;
	Text t;

	void Start(){
		t = GetComponent<Text>();
	}

	float f1 = 0;
	float f2 = 0;
	float f3 = 0;

	public Color best = new Color(0.0f, 1.0f, 1.0f);
	public Color good = new Color(0.0f, 1.0f, 0);
	public Color ok = new Color(1.0f, 1.0f, 0);
	public Color bad = new Color(1.0f,0,0);

    void Update(){
    	f1=f2;
    	f2=f3;

    	f3 = 1.0f/(float)player.dspDelta;
    	float calculated = (f1+f2+f3)/3.0f;
    	t.text = calculated.ToString("0.00") + " DSPfps";

    	if(calculated > 60){
    		t.color = best;
    	} else if(calculated > 20){
    		t.color = Color.Lerp(good, best, (calculated-20)/20.0f);
    	} else if (calculated > 20){
    		t.color = Color.Lerp(ok, good, (calculated-20)/20.0f);
    	} else {
    		t.color = Color.Lerp(bad, ok, calculated/20.0f);
    	}
    }
}
