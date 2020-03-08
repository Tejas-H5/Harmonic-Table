using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInScript : MonoBehaviour {
	public float time = 1;

	bool _isAnimating = false;
	Coroutine _c;

    public void OnEnable(){
        var g = GetComponent<CanvasGroup>();
        g.alpha = 1;
    }

    public void Fade(bool t){
    	gameObject.SetActive(true);
    	if(_isAnimating)
    		StopCoroutine(_c);

    	_c = StartCoroutine(FadeCoroutine(t));
    }

    IEnumerator FadeCoroutine(bool b){
    	var g = GetComponent<CanvasGroup>();
    	float t = 0;
    	while(t < time){
    		t += Time.deltaTime;

    		g.alpha = b ? (t/time) : (1.0f-t/time);
    		yield return new WaitForEndOfFrame();
    	}

        if(!b){
            gameObject.SetActive(false);
        }

    	g.alpha = 1;
    }
}
