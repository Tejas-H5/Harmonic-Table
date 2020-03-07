using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note {
	//frequency of the note.
	public float frequency = 0.0f;
	//how long the note is held for
	public float duration = 0.0f;

	//when the note is to be played
	public double absoluteTime = 0.0f;

	//time after previous note that this one starts in the file. this one not that important tbh
	public float deltaTime = 0.0f;
}
