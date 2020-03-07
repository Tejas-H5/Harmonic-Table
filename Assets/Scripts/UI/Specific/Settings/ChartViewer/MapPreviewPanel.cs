using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPreviewPanel : MonoBehaviour {
	[SerializeField] Text _mapNameTextbox;
	public TrackListGet trackListGetter;

	string _mapName;
	public string mapName{
		get => _mapName;
		set {
			_mapName = value;
			_mapNameTextbox.text = value;
		}
	}

	// more stuff probably
	public void UpdateTrackPanel(){
		trackListGetter.SetCurrentMap(_mapName);
	}
}
