using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}
 
public class UIImageCycle : MonoBehaviour {
    public Sprite[] images;
    int _imageIndex = 0;
    public FloatEvent onChange;

    public int currentImage{
        get{
            return _imageIndex;
        }

        set{
            _imageIndex = value;
            if(_imageIndex < 0){
                _imageIndex = images.Length - 1;
            } else if (_imageIndex >= images.Length){
                _imageIndex = 0;
            }
            GetComponent<Image>().sprite = images[_imageIndex];
        }
    }

    void Awake(){
        currentImage = _imageIndex;
    }

    public void Increment(){
        currentImage++;
        onChange.Invoke(currentImage);
    }

    public void SetImage(float f){
        currentImage = Theory.RoundFloat(Mathf.Clamp(f, 0,images.Length - 1));
    }
}
