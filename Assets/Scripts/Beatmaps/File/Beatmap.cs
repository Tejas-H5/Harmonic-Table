using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using NAudio.Midi;

public class Beatmap {
    //This part will be all static
#if UNITY_EDITOR
    static string _path = @".\MidiFiles\";
#elif UNITY_STANDALONE
    static string _path = @".\MidiFiles\";
#elif UNITY_ANDROID
    static string _path = @"/storage/emulated/0/HarmonicTable/MidiFiles/";
#else
    static string _path = Application.persistentDataPath;
#endif

    //create a beatmap from a filepath to a midi file
    public static Beatmap FromMidi(string name) {
        return new Beatmap(new MidiFile(_path + name + ".mid"), name);
    }

    public static string beatmapPath {
        get => _path;
    }

    string _name;

    public string beatmapName{
        get => _name;
    }

	List<BeatmapTrack> _tracks;

	//ticks per quarter note
	int _tpqn;

	//microseconds per quarter note, changes throughout the track
	int _mspqn;

	//the total time of the track in microseconds
	long _totalTime;

	public double duration{
		get => _totalTime / 1000000.0;
	}

	public void ResetPosition(){
		foreach(var trk in _tracks){
			trk.ResetPosition();
		}
	}
    
    public int numTracks {
    	get => _tracks.Count;
    }

    public BeatmapTrack GetTrack(int i){
    	if(i < 0) return null;
    	if(i >= _tracks.Count) return null;

    	return _tracks[i];
    }

    //create an empty beatmap
    public Beatmap() {
		_tracks = new List<BeatmapTrack>();
    }

    //create a beatmap from an existing MIDI file. The 
    private Beatmap(MidiFile file, string name){
        _name = name;
    	_tracks = new List<BeatmapTrack>();
    	if(file == null)
    		return;

        _tpqn = file.DeltaTicksPerQuarterNote;

        //get all tempo events in the file in sorted order. 
        List<TempoEventInfo> tempoEvents = GetTempoEvents(file);

        //zero the timers
        _totalTime = 0;

        for(int i = 0; i < file.Tracks; i++){
        	var track = file.Events[i];

        	BeatmapTrack currentTrack = AddTrack();
        	//currentTrack.startTime = track.StartAbsoluteTime;

    		_mspqn = 500000;
    		long timeInTicks = 0;
        	long time = 0;
        	long totalTrackTime = 0;

        	//used to get time between NoteOn events (ignoreing other events)
    		long lastNoteDeltaTime = 0;


    		int currentTempoEvent = 0;

    		//Key signature
			int sharpsFlats = 0;
			int majorMinor = 1;


        	foreach(var midiEvent in track){
				lastNoteDeltaTime += TicksToMicroseconds(midiEvent.DeltaTime);

        		//Check if we found a tempo event at this time, and if so update the tempo
        		timeInTicks += midiEvent.DeltaTime;
        		if(currentTempoEvent < tempoEvents.Count){
					if(timeInTicks >= tempoEvents[currentTempoEvent].absoluteTime){
						_mspqn = tempoEvents[currentTempoEvent].mspqn;
						currentTempoEvent++;
					}
					
        		}
				
        		//Process each individual event
	            switch(midiEvent.CommandCode) 
	            {
		            case MidiCommandCode.NoteOn:{
		            	var noteEvent = (NoteEvent)midiEvent;
				    	if(noteEvent.Velocity > 0){
				    		var noteOnEvent = (NoteOnEvent)noteEvent;
					    	Note note = new Note();

					    	note.frequency = Theory.NoteToHertz(Theory.MidiNote(noteOnEvent.NoteNumber, sharpsFlats*majorMinor));

					    	int microsecondDuration = TicksToMicroseconds(noteOnEvent.NoteLength);
					    	note.duration = microsecondDuration / 1000000.0f;

					    	long microsecondDeltaTime = lastNoteDeltaTime;
					    	note.deltaTime = microsecondDeltaTime/1000000.0f;
					    	
					    	//needs to be zeroed as we have just processed this deltaTime
					    	lastNoteDeltaTime = 0;


							time += microsecondDeltaTime;
							note.absoluteTime = time/1000000.0;

					    	//Add the note to the end of the current track
					    	currentTrack.notes.Add(note);

							//the total time needs to account for the durations of the notes as well as their starting points
							long upperBound = time + microsecondDuration;
							if(upperBound > totalTrackTime){
								totalTrackTime = upperBound;
							}
			            }
			            break;
		            }
		            case MidiCommandCode.MetaEvent:{
		            	var metaEvent = (MetaEvent)midiEvent;
		                switch(metaEvent.MetaEventType){
							case MetaEventType.SequenceTrackName:{
								currentTrack.trackName = ((TextEvent)metaEvent).Text;
								break;
							}
							case MetaEventType.TrackInstrumentName:{
								currentTrack.instrumentName = ((TextEvent)metaEvent).Text;
								break;
							}
							case MetaEventType.KeySignature:{
								var kse = ((KeySignatureEvent)metaEvent);
								sharpsFlats = kse.SharpsFlats;
								majorMinor = kse.MajorMinor;
								break;
							}
						}
		                break;
		            }
	            }
        	}
        	//a track has just been read.

            //remove blank tracks
            if(currentTrack.Speed()<1){
                RemoveTrack();
            }

        	//the total song time is just the max of all the track times
        	if(totalTrackTime > _totalTime){
        		_totalTime = totalTrackTime;
        	}
        }
    }

    //get every tempoEvent from a midifile. used in a constructor
    List<TempoEventInfo> GetTempoEvents(MidiFile file){
    	var tempoEvents = new List<TempoEventInfo>();

    	for(int i = 0;  i < file.Tracks; i++){
    		//Add info of every TempoEvent to our list of TempoEventInfos
    		foreach(var e in file.Events[i]){
    			if(e.CommandCode != MidiCommandCode.MetaEvent)
    				continue;
    			if(((MetaEvent)e).MetaEventType != MetaEventType.SetTempo)
    				continue;

    			var tempoEvent = (TempoEvent)e;
    			tempoEvents.Add(new TempoEventInfo(tempoEvent.AbsoluteTime, tempoEvent.MicrosecondsPerQuarterNote));
    		}
    	}

    	tempoEvents.Sort();
    	return tempoEvents;
    }

    public BeatmapTrack AddTrack(){
    	var track = new BeatmapTrack();
    	_tracks.Add(track);
    	return track;
    }

    public void RemoveTrack(){
        if(_tracks.Count > 0)
            _tracks.RemoveAt(_tracks.Count - 1);
    }

    //used while converting a midifile into a Beatmap
    int TicksToMicroseconds(int ticks){
    	return ticks*(_mspqn/_tpqn);
    }
}

