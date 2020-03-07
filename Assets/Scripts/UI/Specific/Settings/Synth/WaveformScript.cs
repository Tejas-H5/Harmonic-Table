using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveformScript : MonoBehaviour {
    public GameObject pixel;
    public int numSamples = 100;
    public OscilatorSettings settings;
    RectTransform[] _points;
    bool _locked = false;
    public void Lock(){
        _locked = true;
    }

    public void Unlock(){
        _locked = false;
        Invalidate();
    }

    void Awake(){
        float w = (transform as RectTransform).rect.width;
        _points = new RectTransform[numSamples-1];
        for(int i = 1; i < numSamples; i++){
            var trf = Instantiate(pixel, transform).transform as RectTransform;
            trf.anchorMin = new Vector2((i/(float)numSamples),0);
            trf.anchorMax = trf.anchorMin;
            trf.anchoredPosition = Vector2.zero;
            _points[i-1] = trf;
        }
    }

    void Start(){
        Invalidate();
    }

    public void Invalidate(){
        if(_locked)
            return;
        if(_points==null)
            return;
        float h = (transform as RectTransform).rect.height/2.0f;
        for(int i = 0; i < _points.Length; i++){
            var point = _points[i];
            float t = i/(float)_points.Length;
            Vector2 pos = point.anchorMin;
            pos.y = (0.5f * settings.Evaluate(t)/settings.vol) + 0.5f;
            point.anchorMin = pos;
            point.anchorMax = pos;
        }
    }
}
