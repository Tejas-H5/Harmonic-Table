using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Events;

public class TrackListGet : MonoBehaviour {
	List<GameObject> panels = new List<GameObject>();

	public GameObject trackPreviewPanel;

	public Transform loadingCircle;
	public Text findingOutput;

	bool _loading = false;

	string _loadedSongName = "";
    
    public UnityEvent tracklistChanged;

	private async void SongPlayerLoad(string name){
        SongPlayer player = SongPlayer.instance;

		if(name!=_loadedSongName){
			_loading = true;
			findingOutput.text = "Loading beatmap...";
			await Task.Run(() => player.LoadBeatmap(name));
			_loadedSongName = name;

			_loading = false;
            tracklistChanged.Invoke();
		}

		findingOutput.text = "["+name+"] "+ player.loadedBeatmap.numTracks + " non-empty tracks";

		UpdatePanels();
	}

    public void SetCurrentMap(string name){
        SongPlayer player = SongPlayer.instance;

    	if(player.isPlaying){
    		findingOutput.text = "Can't load anything while playing";
    		return;
    	}

    	foreach(var ob in panels){
    		ob.SetActive(false);
    	}
        print("yeet");
		SongPlayerLoad(name);
    }

    public void ToggleAll(){
        SongPlayer player = SongPlayer.instance;

    	if(_loading)
    		return;
    	bool b = !panels[0].GetComponent<TrackPreviewPanel>().isEnabled;
    	for(int i = 0; i < player.loadedBeatmap.numTracks; i++){
    		panels[i].GetComponent<TrackPreviewPanel>().isEnabled = b;
    	}
    }

    private void UpdatePanels(){
        SongPlayer player = SongPlayer.instance;

    	float padding = 5;
    	float y = padding;
    	float lastHeight = 0;
    	for(int i = 0; i < player.loadedBeatmap.numTracks; i++){
    		var trk = player.loadedBeatmap.GetTrack(i);
    		GameObject ob;
    		if(i < panels.Count){
    			ob = panels[i];
    			ob.SetActive(true);
    		} else {
    			ob = Instantiate(trackPreviewPanel, transform);
    			panels.Add(ob);
    		}

    		var panel = ob.GetComponent<TrackPreviewPanel>();
    		panel.trackIndex = i;
            panel.trackListGet = this;
    		panel.isEnabled = false || panel.isEnabled;
			panel.trackName = trk.trackName;
			panel.instrumentName = trk.instrumentName;
			panel.speed = (60*trk.Speed()).ToString("0.00") + "npm";
			double dur = trk.duration;
			panel.time = Theory.DurationToString(dur);

			var rtf = panel.transform as RectTransform;
			rtf.anchoredPosition = new Vector2(0, -y);
			lastHeight = rtf.rect.height;
			y += lastHeight + padding;
    	}

    	(transform as RectTransform).sizeDelta = new Vector2(0, y + lastHeight + padding);
    }

    public void SetPlaying(int i, bool v){
        SongPlayer player = SongPlayer.instance;

        player.SetPlaying(i,v);

        if(!_triggered){
            StartCoroutine(InvokeEvent());
        }
    }

    bool _triggered = false;
    IEnumerator InvokeEvent(){
        _triggered = true;
        yield return new WaitForEndOfFrame();
        tracklistChanged.Invoke();
        _triggered = false;        
    }

    void Update(){
    	if(_loading){
    		if(!loadingCircle.gameObject.activeSelf){
    			loadingCircle.gameObject.SetActive(true);
    		}

    		loadingCircle.rotation *= Quaternion.AngleAxis(5 * Time.deltaTime, new Vector3(0, 0, 1));
		} else {
			if(loadingCircle.gameObject.activeSelf){
				loadingCircle.gameObject.SetActive(false);
			}
		}
    }
}
