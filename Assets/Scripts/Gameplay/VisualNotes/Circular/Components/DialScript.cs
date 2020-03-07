using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialScript : MonoBehaviour {
	[SerializeField] Image dial1;

	public Color zeroCol;
	public Color oneCol;

    public void SetFill(float f){
    	dial1.fillAmount = f;
    	dial1.color = Color.Lerp(zeroCol, oneCol, f*f*f);
    }
}
