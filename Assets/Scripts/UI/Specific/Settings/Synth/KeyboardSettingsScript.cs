using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardSettingsScript : MonoBehaviour
{
    public Instrument keyboard;

    public OscilatorSettings oscSettings;
    public Wave wave1;
    public Wave wave2;

    public WaveformScript display;
    public UIScrollbarInput attackSlider;
    public UIScrollbarInput decaySlider;
    public UIScrollbarInput chordsizeSlider;

    void Awake(){
        attackSlider.SetValuePassive(oscSettings.attack);
        print(oscSettings.attack);
        decaySlider.SetValuePassive(oscSettings.decay);
        chordsizeSlider.SetValuePassive(keyboard.chordSize);
    }

    void OnEnable(){
        Invalidate();
    }

    public void Invalidate(){
        display.Lock();

        oscSettings.SetAttack(attackSlider.Value);
        oscSettings.SetDecay(decaySlider.Value);
        keyboard.SetChordSize(chordsizeSlider.Value);

        //FOR LATER
        display.Unlock();
    }
}
