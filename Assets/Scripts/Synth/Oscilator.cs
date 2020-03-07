using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator  {
    //Local state
    public float frequency = 440;
    float _gain = 0.0f;
    float _phase = 0.0f;
    bool _signal = false;

    public void Reset(){
        _gain = 0.0f;
        _phase = 0.0f;
        _signal = false;
    }

    //Global settings
    public OscilatorSettings settings;

    //is it getting a signal this frame?
    public bool pressed{
        get{
            return _signal;
        }
    }

    public void Press(){
        _signal = true;
    }

    //is it resonating this frame?
    public bool activated {
        get {
            return (_gain > 0.01f)||_signal;
        }
    }  

    //volume \in [0,1]
    public float gain{
        get => _gain;
    }

    float sign(float a){
        return a > 0.0f ? 1.0f : -1.0f;
    }

    float tween(float a, float b, float t, TweenType type){
        switch(type){
            case TweenType.Exponential: {
                return Mathf.Lerp(a,b,t);
            }
            default:{
                return Mathf.Clamp(a + sign(b-a)*t,a,b);
            }
        }
    }

    //regulates the oscilator's volume.
    public void Advance(){
        if(_signal){
            _gain = tween(_gain , 1.0f, (1.0f/settings.attack) * Time.deltaTime, TweenType.Exponential);
//            Debug.Log(_gain);
        } else {
            _gain = tween(_gain , 0.0f, (1.0f/settings.decay) * Time.deltaTime, TweenType.Exponential);
        }
        _signal = false;
    }

    public float GetNext(){
        _phase += frequency / settings.sampleRate;;
        if(_phase > 1.0f){
            _phase %= 1.0f;
        }
        
        float sample = settings.Evaluate(_phase);

        return _gain * sample;
    }
}

public enum TweenType{
    Linear,
    Exponential
}