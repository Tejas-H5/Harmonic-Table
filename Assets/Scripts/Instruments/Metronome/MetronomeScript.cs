using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Fully self-contained, won't respond to songPlayer inputs
//Most of the code is from Unity's dspTime example but a lot has been changed
public class MetronomeScript : MonoBehaviour {
	// metronome tempo
    public double bpm = 140.0F;
    // metronome volume
    public float gain = 0.5F;

    //time signature
    public int signatureHi = 4;
    public int signatureLo = 4;

    // high and lo beep
    public float fLo;
    public float fHi;
    //small changes to this value make a huge difference
    public float decay;

    //the time of the next tick with respect to global time
    private double nextTick = 0.0F;

    //metronome tick generation
    private float amp = 0.0F;
    private float phase = 0.0F;

    //audiosettings sample rate
    private double sampleRate = 0.0F;

    //which beat is it?
    private int accent;

    public VisualNote visNote;
    Note _boundNote;

    public Text holdTime;
    public Text holdStart;
    public Text holdEnd;
    public Text holdTiming;
    public Text holdScore;

    void Start(){
    	_boundNote = new Note();
        visNote.Bind(_boundNote);
    }

    void OnEnable() {
        accent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        Debug.Log("Initialized metronome, start tick is " + startTick);
    }

    void OnAudioFilterRead(float[] data, int channels) {
    	// samples per seconds * (seconds per beat) * (4 / quarterNotesPerBeat) 
    	// =>(samples per beat) * (Beats Per note) => samples per note
        double samplesPerTick = sampleRate * (60.0F / bpm) * (4.0F / signatureLo);

        //(the current time in seconds) * (samples per second) => the current sample
        double sample = AudioSettings.dspTime * sampleRate;

        int dataLen = data.Length / channels;
        for(int n = 0; n < dataLen; n++) {
        	//generate the sample
            float x = amp * Mathf.Sin(phase);
            //copy it to all channels
            for(int i = 0; i < channels; i++){
            	data[n * channels + i] += x;
            }

            //if this sample is greater than NextTick, we need to update the accent, and reset the tick
            while (sample + n >= nextTick) {
                nextTick += samplesPerTick;
                amp = gain;

                accent++;
                if (accent > signatureHi) {
                    accent = 1;
                }

                if(accent == signatureHi-1){
                    visNote.ResetTiming();
                	_boundNote.absoluteTime = (nextTick+samplesPerTick)/sampleRate;
                    _boundNote.duration = 0.35f*(float)(samplesPerTick/sampleRate);
                	amp *= 0.5f;
                }

                //Debug.Log("Tick: " + accent + "/" + signatureHi);
            }

            if(accent==1){
				phase += (float)(fHi/sampleRate);
        	} else {
            	phase += (float)(fLo/sampleRate);
        	}

            amp *= decay;
        }
    }

    int _counter = 0;
    void Update(){
    	visNote.Interpolate(AudioSettings.dspTime);

        _counter++;
        if(_counter==5){
            _counter = 0;
            holdTime.text = (visNote.holdEnd - visNote.holdStart).ToString();
            holdStart.text = (visNote.holdStartNorm).ToString();
            holdEnd.text = (visNote.holdEndNorm).ToString();
            holdTiming.text = BeatmapTiming.GetTimingString(visNote.holdStartNorm, visNote.holdEndNorm);
            holdScore.text = (BeatmapTiming.GetScore(visNote.holdStartNorm, visNote.holdEndNorm)).ToString();
            if(visNote.isDead){
                holdScore.text = "|" + holdScore.text + "|";
                holdTiming.text = "|" + holdTiming.text + "|";
            }
        }
    }
}
