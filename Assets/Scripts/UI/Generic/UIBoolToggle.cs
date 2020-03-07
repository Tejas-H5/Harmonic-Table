using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool>{}

public class UIBoolToggle : MonoBehaviour {
    bool _value = false;
    [SerializeField] GameObject trueObj;
    [SerializeField] GameObject falseObj;
    public bool val {
        get{
            return _value;
        }
        set{
            this._value = value;
            trueObj.SetActive(_value);
            falseObj.SetActive(!_value);
        }
    }

    public BoolEvent onChange;

    public void SetValue(bool b){
        val = b;
        onChange.Invoke(val);
    }

    public void Toggle(){
        val = !val;
        onChange.Invoke(val);
    }
}
