using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//used to store tempo change event information in the beatmap
public class TempoEventInfo : IComparable<TempoEventInfo>{
	//the time in MIDI ticks that this event executes
	public long absoluteTime = 0;
	//the new tempo that it sets
	public int mspqn = 0;

	//basic constructor
	public TempoEventInfo(long a, int b){
		absoluteTime = a;
		mspqn = b;
	}

	//used to sort the list of tempo events
	public int CompareTo(TempoEventInfo b){
		return absoluteTime.CompareTo(b.absoluteTime);
	}
}