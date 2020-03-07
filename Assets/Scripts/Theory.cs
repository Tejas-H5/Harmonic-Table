using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theory {	
	public static float NoteToHertz(float note){
		return 440.0f * Mathf.Pow(2,(note-69)/12.0f);
	}

	public static float HertzToNote(float hertz){
		return 69.0f + (12.0f * Mathf.Log(hertz / 440.0f,2.0f));
	}

	public static float ShiftHertz(float hertz, float semitones){
		return NoteToHertz(HertzToNote(hertz) + semitones);
	}

	//letter should be an int from 0-11, number should be another int from 0-10. But we also want to support float
	public static float LetterNumberNote(float letter, float number){
		return letter + number * (float)_notes.Length;
	}

	//TODO: actually implement this properly
	public static float MidiNote(int note, int keySig){
		return note;
	}

	static string[] _notes = {
		"C",
		"C#",
		"D",
		"D#",
		"E",
		"F",
		"F#",
		"G",
		"G#",
		"A",
		"A#",
		"B",
	};

	public static int RoundFloat(float n){
		if(n%1.0f > 0.5f){
			return (int)n+1;
		} else {
			return (int)n;
		}
	}

	public static int NoteNumber(float note){
		//needs to be an integer division
		return (RoundFloat(note-12)/_notes.Length)+1;
	}

	public static string NoteLetter(float note){
		if(note < 0)
			return "??";
		
		return _notes[RoundFloat(note) % _notes.Length];
	}

	public static string NoteToString(float note){
		return NoteLetter(note) + NoteNumber(note).ToString();
	}

	public static bool IsSharp(float note){
		switch(RoundFloat(note) % _notes.Length){
			case 1:
			case 3:
			case 6:
			case 8:
			case 10:
				return true;				
			default:
				return false;
		}
	}

	static string[] _intervals = {
		"Same note",
		"Semitone",
		"Tone",
		"Minor third",
		"Major Third",
		"Perfect fourth",
		"Tritone",
		"Perfect fifth",
		"Minor sixth",
		"Major sixth",
		"Minor seventh",
		"Major seventh",
		"Perfect octave",
	};

    public static string Interval(float interval){
        return interval.ToString("0.0") + (interval > 0 ? " ↑" : " ↓");
    }

	public static string IntervalName(float interval){
		int gap = RoundFloat(Mathf.Abs(interval));
		if(gap >= _intervals.Length){
			return "Large interval";
		}

		return _intervals[gap] + (gap==0 ? "" : (interval > 0 ? " ↑" : " ↓"));
	}

	public static string Distortion(float note){
		float remainder = Mathf.Abs(note) % 1.0f;
		if(remainder > 0.5f){
			remainder -= 1.0f;
		}
		return remainder.ToString("+0.000;-0.000")+" semitiones";
	}

	//This has nothing to do with music theory
	public static string DurationToString(double dur){
		return (dur/60).ToString("00") +":"+ (dur % 60).ToString("00")+(dur%1.0f).ToString(".00");
	}
}
