using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPressable{
	void Press();
	void Release();
	float RecalcRadius();
}

//to be placed on objects that have another script that is IPressable
public class CircularPressable : MonoBehaviour {
	IPressable _pressObject;

	protected virtual void Awake(){
		_pressObject = GetComponent<IPressable>();
		if(_pressObject == null){
			print("sad");
		}
	}

	bool CheckPress(Vector2 pos){
		return InputChecks.PointInCircle(transform.position, pos, _pressObject.RecalcRadius());
	}

	bool _pressed = false;

	void Press(){
		_pressed = true;
		_pressObject.Press();
	}

    public virtual void Update(){
		//Take input, trigger OnPress if we get pressed
		for(int i = 0; i < Input.touchCount; i++){
			Touch touch = Input.GetTouch(i);
			if(CheckPress(touch.position)){
				Press();
				return;
			}
		}

		//only reaason why this exists is because of PC
		if(Input.touchCount==0){
			if(Input.GetButton("Fire1")){
				if(CheckPress(Input.mousePosition)){
					Press();
					return;
				}
			}
		}

		if(_pressed){
			_pressed = false;
			_pressObject.Release();
		}
	}
}
