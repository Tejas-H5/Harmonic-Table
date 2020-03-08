using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour {
    public Text perfectText;
    public Text goodText;
    public Text okText;
    public Text missText;

    int _perfects;
    int _goods;
    int _oks;
    int _miss;

    public ScoreTracker st;

    public void Update(){
    	if(_perfects!=st.getPerfect()){
    		perfectText.text = st.getPerfect().ToString();
    		_perfects = st.getPerfect();
    	}
    	if(_goods!=st.getGood()){
    		goodText.text = st.getGood().ToString();
    		_goods = st.getGood();
    	}
    	if(_oks!=st.getOk()){
    		okText.text = st.getOk().ToString();
    		_oks = st.getOk();
    	}
    	if(_miss!=st.getMiss()){
    		missText.text = st.getMiss().ToString();
    		_miss = st.getMiss();
    	}
    }
}
