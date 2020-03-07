using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//this class plays songs on any 'Instrument' capable of handling the notes
public class SongPlayer : MonoBehaviour {
	public Instrument instrument;    
    public int maxNotesPerFrame = 300;

    public UnityEvent onSongEnd;

    static SongPlayer _instance;
    public static SongPlayer instance{
        get => _instance;
    }

    void Awake(){
        _instance = this;
    }

    [SerializeField] bool[] playingTracks;

    public bool IsTrackEnabled(int i){
        return playingTracks[i];
    }

    public void SetPlaying(int i, bool val){
        if((i<0)||(i >= playingTracks.Length)){
            return;
        }

        playingTracks[i] = val;

        _recalcStart = true;
        _recalcEnd = true;
    }

    public bool autoplay = true;
    public void SetAutoplay(bool b){
        autoplay = b;
    }

    //the time before which we invoke an instrument note press
    public double invokeForesight = 1.0f;

    //A calibration offset between purely for scoreing. It can be deduced from the very first tap as well
    public double delay = 0.0f;

    Beatmap _beatmap;

    //The list of held-down notes by this automated midi song player
    List<Note> notes;

    int numPlaying = 0;

    double _playSpeed = 1.0f;

    public double playSpeed{
        get => _playSpeed;
    }

    //--------- Playback Modifications (AKA Mods) ---------

    public bool doubleSpeed;
    public bool halfSpeed;
    public bool automatedPlaythrough;
    public bool redundancy;
    public bool reflexes;
    public bool matrix;

    //--- PROPERTIES ----

    public Beatmap loadedBeatmap{
        get => _beatmap;
    }

    public bool isPlaying {
        get => numPlaying > 0;
    }

    bool _isPaused = false;
    public bool isPaused{
        get => _isPaused;
    }

    double _time = -1.0;

    public double songTime{
        get=>_time;
    }

    public double startDelay = 2.0;

    public string currentSongName {
        get{
            if(loadedBeatmap==null){
                return "Song not yet loaded ;-;";
            } else {
                return loadedBeatmap.beatmapName;
            }
        }
    }

    //---- Selection properties ---- 

    bool _recalcStart = true;
    bool _recalcEnd = true;

    double _selectedStart = 0.0;
    double _selectedEnd = 0.0;

    public double selectedStart{
        get {
            if(_recalcStart){
                _selectedStart = _beatmap.duration;

                for(int i = 0; i < _beatmap.numTracks; i++){
                    if(!playingTracks[i])
                        continue;

                    var trk = _beatmap.GetTrack(i);
                    if(trk.startTime < _selectedStart)
                        _selectedStart = trk.startTime;
                }

                _recalcStart = false;
            }

            return _selectedStart;
        }
    }

    public double selectedEnd{
        get {
            if(_recalcEnd){
                _selectedEnd = 0;

                for(int i = 0; i < _beatmap.numTracks; i++){
                    if(!playingTracks[i])
                        continue;

                    var trk = _beatmap.GetTrack(i);
                    if(trk.endTime > _selectedEnd)
                        _selectedEnd = trk.endTime;
                }
                _recalcEnd = false;
            }
            
            return _selectedEnd;
        }
    }

    public double selectedDuration{
        get {
            double res = selectedEnd - selectedStart;
            if(res>0)
                return res;
            return 0;
        }
    }

	public void SetSpeed(float f){
		_playSpeed = (double)(Mathf.Clamp(f,0.1f,10.0f));
	}

    void Start(){
    	notes = new List<Note>();
    	//14 is arbitrary
    	notes.Capacity = 14;
    }

    public void LoadBeatmap(string name){
    	_beatmap = Beatmap.FromMidi(name);

    	playingTracks = new bool[_beatmap.numTracks];
    	for(int i = 0; i < playingTracks.Length; i++){
    		playingTracks[i] = true;
    	}

    	print("Loaded " + _beatmap.beatmapName);
    }

    public void PlaySong(){
    	if(_beatmap == null){
    		print("Nothing loaded.");
    		return;
    	}

        StopSong();

    	print("Now playing " + _beatmap.beatmapName);

        
        _time = -BeatmapTiming.foresight - startDelay;// + _beatmap.StartTime();
        _lastDspTime = AudioSettings.dspTime;

        PlayTracks();
    }

    private void PlayTracks(){
        numPlaying = 0;
        for(int i = 0; i < _beatmap.numTracks; i++){
            if(!playingTracks[i])
                continue;

            numPlaying++;
            var track = _beatmap.GetTrack(i);
            StartCoroutine(PlayNotes(track));
        }
    }

    public void StopSong(){
        StopAllCoroutines();

    	numPlaying = 0;
        _isPaused = false;
    	_beatmap.ResetPosition();
    	notes.Clear();
    }

    public void PauseSong(){
        print("Paused");
        _isPaused = true;

        if(songTime < 0)
            return;

        if(isPaused) 
            return;
    }

    public void ResumeSong(){
        if(!isPlaying){
            PlaySong();
            return;
        }

        _isPaused = false;

        if(!isPaused){
            print("was never paused ");
            return;
        }
    }

    private bool NotYetTime(double t){
        return (songTime < t);
    }

    //keep track of how many notes we make per frame so we can limit this
    int _numNotes = 0;

    //each track is a seperate iEnumerator
    private IEnumerator PlayNotes(BeatmapTrack track){
    	while(isPlaying){
	    	Note n = track.NextNote();
            //check if we used all the notes
	    	if(n == null){
                //wait till the very end to stop the track
                while(NotYetTime(track.endTime)){
                    yield return new WaitForEndOfFrame();
                }
                numPlaying--;
	    		yield break;
	    	}

			//wait untill this note needs to be played/anticipated.
	    	while(NotYetTime(n.absoluteTime - (double)BeatmapTiming.foresight)){
                _numNotes = 0;
    			yield return new WaitForEndOfFrame();
    		}

	    	AnticipatePress(n);
            _numNotes++;

            if(_numNotes > maxNotesPerFrame){
                _numNotes = 0;
                yield return new WaitForEndOfFrame();
            }
    	}
    }

    
    //creates a visual note before the actual note press.
    void AnticipatePress(Note n){
        instrument.CreateVisualNote(n);
        StartCoroutine(Press(n));
    }

    //Presses a note when it is it'_selectedStart time. 
    //Theoretically you could call this on every note in a song at once, but I'd rather not try
    private IEnumerator Press(Note n) {
        while(NotYetTime(n.absoluteTime)){
            yield return new WaitForEndOfFrame();
        }

    	//Add the frequency to the list of notes to hold down
    	notes.Add(n);

    	//This is for if frequency gets removed be Release() faster than Update is able to see it
        //NOT REDUNDANT
        if(autoplay){
    	   instrument.Press(n);
        }

    	//release the key after a certain time
        double absReleaseTime = n.absoluteTime + (double)n.duration;
        while(NotYetTime(absReleaseTime)){
            yield return new WaitForEndOfFrame();
        }

    	Release(n);
    }

	//removes a note from the press-list
    private void Release(Note n) {
    	int i = notes.IndexOf(n);
        if(i<0)
            return;

		notes[i] = notes[notes.Count - 1];
		notes.RemoveAt(notes.Count - 1);
    }

    double _lastDspTime = 0;
    double _dspDeltaTime = 0;

    public double dspDelta{
        get=>_dspDeltaTime;
    }

    void Update() {
        double dspTime = AudioSettings.dspTime;
        _dspDeltaTime = dspTime - _lastDspTime;
        _lastDspTime = dspTime;

    	if(!isPlaying)
    		return;

        if(_isPaused)
            return;

        //limit the lag to 0.2sec per frame max
        //_time += (_dspDeltaTime > 0.2 ? 0.2 : _dspDeltaTime) * _playSpeed;
        _time += Time.deltaTime * _playSpeed;

        if(autoplay){
        	for(int i = 0; i < notes.Count; i++){
    	    	instrument.Press(notes[i]);
        	}
        }

        instrument.UpdateVisualNotes(songTime);
    }
}
