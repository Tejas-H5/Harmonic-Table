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

	List<RectTransform> bars;
	List<float> barHeights;

	void Start(){
		bars = new List<RectTransform>();
		bars.Capacity = numBars;
		barHeights = new List<float>();
		barHeights.Capacity = numBars;

		float r = radius;//(container as RectTransform).rect.height/2;
		for(int i = 0;i < numBars; i++){
			var bar = (Instantiate(barPrefab, container) as GameObject).GetComponent<RectTransform>();
			bars.Add(bar);
			barHeights.Add(0);

			float angle = ((float)i/(float)numBars) * Mathf.PI * 2f;
			bar.anchoredPosition = r * new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
			bar.eulerAngles = new Vector3(0,0,angle * Mathf.Rad2Deg - 90);
		}
	}

	public int prongs = 3;
    public float spinSpeed = 1;

    float angle=0;
	void OnAudioFilterRead(float[] data, int channels) {
		//float scaleFactor = (float)data.Length/(float)barHeights.Count;
		float scaleFactor = ((float)data.Length+channels)/(float)barHeights.Count;

		for(int i = 0; i < barHeights.Count; i++){
			barHeights[i] = 0;
		}

        float vel = 0;
		for(int i = 0; i < data.Length; i+=channels){
            vel += Mathf.Abs(data[i]);
			//map the sample's volume onto our array of barHeights
			int index = (int)(Mathf.Clamp01(Mathf.Abs(data[i])) * ((barHeights.Count-1)/prongs));
            index += (int)(barHeights.Count * angle);

			for(int j = 0; j < prongs; j++){
				barHeights[(index + j*((barHeights.Count-1)/prongs)) % barHeights.Count] += response * Mathf.Abs(data[i]);
			}
		}

        vel/=(float)data.Length;
        angle+=vel*spinSpeed;
        angle %= Mathf.PI*2;
		
		/*
		for(int i = 0; i < barHeights.Count; i++){
			barHeights[i] += Mathf.Abs(data[(int)(i*scaleFactor)+channels]-data[(int)(i*scaleFactor)]);
		}


		for(int i = 0; i < barHeights.Count; i++){
			barHeights[i] *= decay;
			barHeights[i] = Mathf.Clamp(barHeights[i], 0,3);
		}
		*/
    }

	public float response2 = 10;

    void Update(){
    	for(int i = 0; i < barHeights.Count; i++){
    		//bars[i].sizeDelta = new Vector2(barWidth, baseHeight + sampleHeight * (Mathf.Abs(barHeights[i+1]) - Mathf.Abs(barHeights[i])));
    		bars[i].sizeDelta = Vector2.Lerp(bars[i].sizeDelta, new Vector2(barWidth, baseHeight + sampleHeight * barHeights[i]), response2*Time.deltaTime);
    	}
    }
}
