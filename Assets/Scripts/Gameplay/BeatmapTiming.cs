using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//should have been called Beatmap Settings
public class BeatmapTiming : MonoBehaviour {
	//the time this note should appear before the actual note. 
	//this should remain constant throughout an entire song	
	public static float foresight = 0.5f;

	//the scale of the approach circle.
	public static float approachScale = 3.0f;

	public static float timingTolerance = 0.07f;

	public static string GetTimingString(float start, float end){
		TimingResult t = GetTiming(start, end);

		switch(t){
			case TimingResult.Perfect:{
				return 	"Perfect";
			}
			case TimingResult.Good:{
				return 	"Good";
			}
			case TimingResult.Ok:{
				return 	"Ok";
			}
			default:{
				return 	"Miss";
			}
		}
	}

	//start and end need to be normalized
	public static TimingResult GetTiming(float start, float end){
		float s = GetScore(start, end);

		if(s < 0.1f){
			return TimingResult.Miss;
		} else if(s < 0.5f){
			return TimingResult.Ok;
		} else if(s < 0.8f){
			return TimingResult.Good;
		} else {
			return TimingResult.Perfect;
		}
	}

	//user adjustable value to change what is considered an approriate start and end
	public static float universalOffset = 0.0f;

	//start and end need to be normalized
	public static float GetScore(float start, float end){
		float s = 0.5f*(2.0f - Mathf.Abs(start) - Mathf.Abs(1.0f-Mathf.Clamp01(end)));

		return s;
	}
}


public enum TimingResult{
	Perfect,
	Good,
	Ok,
	Miss
}