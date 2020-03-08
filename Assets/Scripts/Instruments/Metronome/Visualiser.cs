using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualiser : MonoBehaviour {
	public GameObject barPrefab;
	public RectTransform container;
	public int numBars = 100;

	public float barWidth = 5;
	public float baseHeight = 5;
	public float sampleHeight = 100;
	public float decay = 3f;
	public float response = 0.1f;
    public float radius = 50.0f;

	List<RectTransform> _bars;
	List<float> _barHeights;

	void Start(){
		_bars = new List<RectTransform>();
		_bars.Capacity = numBars;
		_barHeights = new List<float>();
		_barHeights.Capacity = numBars;

		float r = radius;//(container as RectTransform).rect.height/2;
		for(int i = 0;i < numBars; i++){
			var bar = (Instantiate(barPrefab, container) as GameObject).GetComponent<RectTransform>();
			_bars.Add(bar);
			_barHeights.Add(0);

			float angle = ((float)i/(float)numBars) * Mathf.PI * 2f;
			bar.anchoredPosition = r * new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
			bar.eulerAngles = new Vector3(0,0,angle * Mathf.Rad2Deg - 90);
		}
	}

	public int prongs = 3;
    public float spinSpeed = 1;

    float angle=0;
	void OnAudioFilterRead(float[] data, int channels) {
		//float scaleFactor = (float)data.Length/(float)_barHeights.Count;
		float scaleFactor = ((float)data.Length+channels)/(float)_barHeights.Count;

		for(int i = 0; i < _barHeights.Count; i++){
			_barHeights[i] = 0;
		}

        float vel = 0;
		for(int i = 0; i < data.Length; i+=channels){
            vel += Mathf.Abs(data[i]);
			//map the sample's volume onto our array of _barHeights
			int index = (int)(Mathf.Clamp01(Mathf.Abs(data[i])) * ((_barHeights.Count-1)/prongs));
            index += (int)(_barHeights.Count * angle);

			for(int j = 0; j < prongs; j++){
				_barHeights[(index + j*((_barHeights.Count-1)/prongs)) % _barHeights.Count] += response * Mathf.Abs(data[i]);
			}
		}

        vel/=(float)data.Length;
        angle+=vel*spinSpeed;
        angle %= Mathf.PI*2;
		
		/*
		for(int i = 0; i < _barHeights.Count; i++){
			_barHeights[i] += Mathf.Abs(data[(int)(i*scaleFactor)+channels]-data[(int)(i*scaleFactor)]);
		}


		for(int i = 0; i < _barHeights.Count; i++){
			_barHeights[i] *= decay;
			_barHeights[i] = Mathf.Clamp(_barHeights[i], 0,3);
		}
		*/
    }

	public float response2 = 10;

    void Update(){
    	for(int i = 0; i < _barHeights.Count; i++){
    		//_bars[i].sizeDelta = new Vector2(barWidth, baseHeight + sampleHeight * (Mathf.Abs(_barHeights[i+1]) - Mathf.Abs(_barHeights[i])));
    		_bars[i].sizeDelta = Vector2.Lerp(_bars[i].sizeDelta, new Vector2(barWidth, baseHeight + sampleHeight * _barHeights[i]), response2*Time.deltaTime);
    	}
    }
}
