using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instrument : MonoBehaviour {
	//For use by children keys, etc to detect touches.
	public float chordSize = 1.0f;

	//The object that will spawn notes for us.
	public NoteSpawner noteSpawner;

	public void SetChordSize(float f){
		chordSize = f;
	}
	//A press that must be done every frame. 
	//will press a certain frequency (+- tolerance) on the instrument. Not used with UI elements.
	public abstract void Press(Note n);

	//Can this frequency (+- tolerance) be played on this instrument?
	public abstract bool CanPress(Note n);

	//Releases all notes that are being activated.
	public abstract void ReleaseAllNotes();

	//creates a note that the user can see that will help them play a real note
	public abstract void CreateVisualNote(Note n);

	public virtual void UpdateVisualNotes(double t){
		noteSpawner.UpdateNotes(t);
	}

	protected virtual void SpawnNote(Note n,Vector3 pos, float scale){
		noteSpawner.SpawnNote(n, pos, scale);
	}
}

public class FloatParam{
	public float f = 0.0f;
	public FloatParam(float f){
		this.f = f;
	}
}