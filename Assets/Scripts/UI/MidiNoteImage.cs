using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MidiNoteImage : MonoBehaviour {
	public Text text{
		get => _text;
	}

	public Image image{
		get => _img;
	}

	public RectTransform rt{
		get => _rt;
	}

	RectTransform _rt;

	void Awake(){
		_rt = GetComponent<RectTransform>();
	}

	[SerializeField] Image _img;
	[SerializeField] Text _text;
}
