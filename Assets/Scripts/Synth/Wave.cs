using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
    [SerializeField] float _amp = 0.4f;
    [SerializeField] float _shift = 0.0f;
    [SerializeField] float _period = 1;
    [SerializeField] WaveType _type = WaveType.Sine;

    public float amps { 
        get { return _amp; } 
        set {
            _amp = Mathf.Clamp(value,-1.0f,1.0f);
        }
    }

    public float shift { 
        get { return _shift; } 
        set{
            _shift = Mathf.Clamp(value, 0,1);
        }
    }

    public float period { 
        get { return _period; } 
        set {
            _period = Mathf.Clamp(value,0.1f,8.0f);
        }
    }

    public WaveType type { 
        get { return _type; }
        set { _type = value; }
    }
    
    public void SetType(float f){
        type = (WaveType)Theory.RoundFloat(f);
    }

    public float Evaluate(float t) {
        t = ((t+_shift) * _period);
        float res;
        switch(_type) {
            case WaveType.Triangle:{
                res=Mathf.PingPong(4*t,2.0f)-1.0f;
                break;
            }
            case WaveType.Sawtooth:{
                res = (2*t%2)-1;
                break;
            }
            case WaveType.Square:{
                res = (2*t%2)-1 > 0 ? 1.0f : -1.0f;
                break;
            }
            default:{
                res = Mathf.Sin(Mathf.PI * 2.0f * t);
                break;
            }
        }
        return _amp * res;
    }
}

public enum WaveType{
    Sine,
    Square,
    Triangle,
    Sawtooth
}
