using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpawner : MonoBehaviour {
    Stack<VisualNote> _pool;
    List<VisualNote> _activeNotes;
    public RectTransform noteContainer;

    //Prefabs
    public GameObject holdNote;

    void Awake(){
        _pool = new Stack<VisualNote>();
        _activeNotes = new List<VisualNote>();
    }

    public VisualNote InstantiateNote(Note data){
        VisualNote note;
        if(_pool.Count == 0){
            note = CreateNote();
            print("Instantiated something");
        } else {
            note = _pool.Pop();
        }

        note.gameObject.SetActive(true);
        InitializeNote(note, data);
        _activeNotes.Add(note);
        return note;
    }

    public void DestroyNote(VisualNote note){
        note.gameObject.SetActive(false);
        _activeNotes.Remove(note);
        _pool.Push(note);

        //Some gameplay regarding note timing
        TimingResult t = BeatmapTiming.GetTiming(note.holdStartNorm, note.holdEndNorm);
        ScoreTracker.instance.Increment(t);
    }

    public void SpawnNote(Note note, Vector3 pos, float scale){
        var ob = InstantiateNote(note);
        ob.SetTransform(pos,scale);
    }

    //Initializes a visual note based on note data
    protected void InitializeNote(VisualNote note, Note data){
        note.Bind(data);
    }   

    //Creates an actual note object. With Instantiate, probably
    protected VisualNote CreateNote(){      
        return Instantiate(holdNote, noteContainer).GetComponent<VisualNote>();
    }

    bool _deleteAll = false;
    void Update(){
        if(!_deleteAll){
            _deleteAll = true;
        } else {
            ClearAllActive();
        }
    }

    void ClearAllActive(){
        for(int i = 0; i < _activeNotes.Count; i++){
            _activeNotes[i].gameObject.SetActive(false);
            _pool.Push(_activeNotes[i]);
        }
        _activeNotes.Clear();
    }

    public void UpdateNotes(double t){
        _deleteAll = false;
        for(int i = 0; i < _activeNotes.Count; i++){
            var n = _activeNotes[i];
            n.Interpolate(t);
            if(n.isDead){
                DestroyNote(n);
                i--;
            }
        }
    }
}
