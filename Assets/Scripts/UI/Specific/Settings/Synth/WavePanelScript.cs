using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavePanelScript : MonoBehaviour {
    public UIScrollbarInput amp;
    public UIScrollbarInput shift;
    public UIScrollbarInput period;

    public Text ampText;
    public Text shiftText;
    public Text periodText;

    public UIImageCycle type;

    public Wave wave;

    public WaveformScript waveform;

    void Awake(){
        print("awoken");
        getValues();
    }

    void getValues(){
        amp.SetValuePassive(wave.amps);
        shift.SetValuePassive(wave.shift);
        period.SetValuePassive(wave.period);

        ampText.text = wave.amps.ToString("0.00");
        shiftText.text = wave.shift.ToString("0.00");
        periodText.text = wave.period.ToString("0.00");
        type.currentImage = (int)wave.type;
    }

    void setValues(){
        wave.amps = amp.Value;
        wave.shift = shift.Value;
        wave.period = period.Value;
        wave.SetType(type.currentImage);
    }

    public void Invalidate(){
        print("invalidate");
        setValues();
        getValues();

        waveform.Invalidate();
    }
}
