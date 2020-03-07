using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdateBool : MonoBehaviour {
    public string trueStr;
    public string falseStr;

    public void SetText(bool b){
        GetComponent<Text>().text = b ? trueStr : falseStr;
    }
}
