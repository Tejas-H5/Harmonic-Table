using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilatorSettings : MonoBehaviour
{
    [SerializeField] float _attack = 0.4f;
    [SerializeField] float _decay = 0.5f;

    [SerializeField] float _vol = 0.2f;
    [SerializeField] Wave _w1;
    [SerializeField] Wave _w2;

    public float decay { get { return _decay; } }
    public float attack { get { return _attack; } }
    public float vol { get { return _vol; } }

    float _sampleRate;
    public float sampleRate{ get { return _sampleRate; } }
    
    public void Awake(){
        _sampleRate = AudioSettings.outputSampleRate;
    }

    public void SetDecay(float val){
        _decay = Mathf.Clamp(val,0.0f,20.0f);
    }
    public void SetAttack(float val){
        _attack = Mathf.Clamp(val,0.0f,20.0f);
    }

    public float Evaluate(float t){
        return _vol*(_w1.Evaluate(t) + _w2.Evaluate(t));
    }
}
