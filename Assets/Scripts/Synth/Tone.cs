using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tone {
    [SerializeField] List<Wave> _waves = new List<Wave>();
    [SerializeField] string _name;

    public string name {
        get { return _name; }

        set{
            if(value.Length <= 1)
                return;
            _name = value;
        }
    }

    public List<Wave> waves { get { return _waves; } }

    public float Evaluate(float t){
        float res = 0.0f;
        for(int i = 0; i < _waves.Count; i++){
            res += _waves[i].Evaluate(t)/(float)_waves.Count;
        }
        return res;
    }
}
