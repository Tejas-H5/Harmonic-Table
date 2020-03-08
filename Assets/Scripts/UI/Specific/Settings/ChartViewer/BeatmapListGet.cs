using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatmapListGet : MonoBehaviour {
	public GameObject mapFilePreviewPanel;

    public TrackListGet trackListGetter;

	List<GameObject> panels = new List<GameObject>();	

	public float padding = 5;

    public Text errorOutput;

    bool _firstTime = true;
    
    void OnEnable(){
        GetBeatmaps();
    }

    public void GetBeatmaps(){
    	var mapPaths = BasicFileIO.EnumerateFiles(Beatmap.beatmapPath, "mid");
    	if(mapPaths==null){
            errorOutput.enabled = true;
            errorOutput.text = BasicFileIO.exception;
    		return;
        } else {
            errorOutput.enabled = false;
        }

    	float y = padding;
    	float lastHeight = 0;

        int i = 0;
    	foreach(string path in mapPaths){
    		int rootNameLength = Beatmap.beatmapPath.Length;
    		// the -4 at the end is to remove the ".mid" at the end of the string
    		string fileName = path.Substring(rootNameLength, path.Length - rootNameLength -4);

            GameObject ob;

            if(i < panels.Count){
                ob = panels[i];
            } else {
        		//create a panel as a child of this object
        		ob = Instantiate(mapFilePreviewPanel, transform) as GameObject;
        		panels.Add(ob);
            }

    		var preview = ob.GetComponent<MapPreviewPanel>();

    		//initialize the panel's display variables
    		preview.mapName = fileName;
            preview.trackListGetter = trackListGetter;

    		//position the panel accordingly
    		var rtf = preview.transform as RectTransform;

    		rtf.anchoredPosition = new Vector2(0,-y);
    		lastHeight = rtf.rect.height;

            if((i==0)&&(_firstTime)){
                _firstTime = false;
                trackListGetter.SetCurrentMap(preview.mapName);
            }

    		y += lastHeight + padding;
            i++;
    	}

        if(i==0){
            errorOutput.enabled = true;
            errorOutput.text = "Add some midi files (.mid) to the directory [" + 
            System.IO.Path.GetFullPath(Beatmap.beatmapPath) + 
            "] and then they'll appear here";
        }

        //delete the unreused panels
        int n = panels.Count;
        for(int k = 0; i + k < n; k++){
            Destroy(panels[n-1 - k]);
            panels.RemoveAt(n-1 - k);
        }

    	//resize ourself to match the height of our contents
    	//only works if the anchor points are (<-stretch->, top)
    	(transform as RectTransform).sizeDelta = new Vector2(0, y + lastHeight + padding);
    }
}
