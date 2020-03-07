using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardSettingsScript : MonoBehaviour
{
    public OscilatorSettings oscSettings;
    public Wave wave1;
    public Wave wave2;

    public WaveformScript display;
    public Slider attackSlider;
    public Slider decaySlider;
    public Slider sustainSlider;
    public Slider releaseSlider;

    public Slider amp2;
    public Slider shift2;
    public Slider repeat2;
    public UIImageCycle type2;

    void Start(){
        Invalidate();
    }

    void OnEnable(){
        Invalidate();
    }

    public void Invalidate(){
        display.Lock();
        attackSlider.value = oscSettings.attack;
        decaySlider.value = oscSettings.decay;
        //FOR LATER
        display.Unlock();
    }
}
