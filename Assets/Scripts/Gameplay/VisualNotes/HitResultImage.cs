using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitResultImage : MonoBehaviour {
	public Image img;
	public Sprite[] sprites;
	public float time = 0.2f;

	float _t = 0;
	Coroutine _anim;

	void OnEnable(){
		img.enabled = false;
	}

	IEnumerator Animation(){
		img.enabled = true;
		_t = 0;
		//print("yeet");
		
		while(_t < time){
			_t += Time.deltaTime;

			float lf = _t/time;

			float a = 1.0f;

			if(lf<0.5f){
				a = Mathf.Lerp(0f,1f,lf/0.5f);
			}

			Color c = img.color;
			c.a = a;
			img.color = c;
			
			yield return new WaitForEndOfFrame();
		}

		img.enabled = false;
	}

    public void Animate(TimingResult res){
    	int index = (int)res;

    	img.sprite = sprites[index];

    	switch(res){
			case TimingResult.Perfect:{
				img.color = new Color(0,1,1,0);
				break;
			}
			case TimingResult.Good:{
				img.color = Color.green;
				break;
			}
			case TimingResult.Ok:{
				img.color = Color.yellow;
				break;
			}
			default:{
				img.color = Color.red;
				break;
			}
		}

    	if(_t > 0){
    		StopCoroutine(_anim);
    	}

    	_anim = StartCoroutine(Animation());
    }
}
