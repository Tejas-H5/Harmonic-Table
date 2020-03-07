using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisable : MonoBehaviour {
    public void Enable(bool b){
        gameObject.SetActive(b);
    }

    public void Disable(bool b){
        gameObject.SetActive(!b);
    }
}
