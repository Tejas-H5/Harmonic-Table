using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIUpDown : MonoBehaviour {
    public FloatEvent onValueChange;
    [SerializeField] TextUpdate readout;
    [SerializeField] int _value = 0;
    
    public int val{
        get { return _value; }
        set{
            _value = value;
            if(val>max){
                val = loop ? min : max;
            } else if(val<min){
                val = loop ? max : min;
            }
            readout.SetText(_value);
        }
    }

    public int min;
    public int max;
    public bool loop = false;

    public void SetValue(float f){
        val = Theory.RoundFloat(f);
        onValueChange.Invoke(val);
    }

    public void Increment(){
        val++;
        onValueChange.Invoke(val);
    }

    public void Decrement(){
        val--;
        onValueChange.Invoke(val);
    }
}
