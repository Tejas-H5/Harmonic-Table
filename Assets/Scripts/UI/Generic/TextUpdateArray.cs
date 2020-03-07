using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdateArray : TextUpdate {
    public string[] options;

    //will round to the nearest integer
    public override void SetText(float value){
        var textob = GetComponent<Text>();

        int i = Theory.RoundFloat(value);
        if((i < 0)||(i >= options.Length)){
            textob.text = "???";
        } else {
            textob.text = options[i];
        }
    }
}
