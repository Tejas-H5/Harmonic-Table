using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour {
    public Text perfectText;
    public Text goodText;
    public Text okText;
    public Text missText;

    int perfects;
    int goods;
    int oks;
    int misss;

    public ScoreTracker st;

    public void Update(){
    	if(perfects!=st.getPerfect()){
    		perfectText.text = st.getPerfect().ToString();
    		perfects = st.getPerfect();
    	}
    	if(goods!=st.getGood()){
    		goodText.text = st.getGood().ToString();
    		goods = st.getGood();
    	}
    	if(oks!=st.getOk()){
    		okText.text = st.getOk().ToString();
    		oks = st.getOk();
    	}
    	if(misss!=st.getMiss()){
    		missText.text = st.getMiss().ToString();
    		misss = st.getMiss();
    	}
    }
}
