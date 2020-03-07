using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour {
    public bool asInt = false;
    public float multiplier;
    public Slider slider;

    public virtual void SetText(float value){
        var textob = GetComponent<Text>();

        if(asInt){
            textob.text = (Theory.RoundFloat(value)).ToString();
        } else {
            textob.text = value.ToString();
        }
    }

    public void UpdateText(){
        float value = slider.value;

        SetText(value);
    }
}