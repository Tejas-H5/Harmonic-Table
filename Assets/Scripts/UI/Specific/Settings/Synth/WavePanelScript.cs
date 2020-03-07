using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavePanelScript : MonoBehaviour {
    public Slider amp;
    public Slider shift;
    public Slider period;

    public Text ampText;
    public Text shiftText;
    public Text periodText;

    public UIImageCycle type;

    public Wave wave;

    public WaveformScript waveform;

    void Start(){
        Invalidate();
    }

    public void Invalidate(){
        wave.amps = (amp.value/5.0f) - 1.0f;
        wave.shift = shift.value/10.0f;
        wave.period = period.value/10f;
        wave.SetType(type.currentImage);

        amp.value = (wave.amps+1.0f)*5;
        shift.value = wave.shift*10;
        period.value = wave.period*10;

        ampText.text = wave.amps.ToString("0.00");
        shiftText.text = wave.shift.ToString("0.00");
        periodText.text = wave.period.ToString("0.00");

        type.currentImage = (int)wave.type;

        waveform.Invalidate();
    }
}
