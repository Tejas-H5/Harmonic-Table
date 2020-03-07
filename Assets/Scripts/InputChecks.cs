using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputChecks : MonoBehaviour {
    public static bool PointInCircle(Vector2 pos1, Vector2 pos2, float rad){
    	return ((pos1 - pos2).sqrMagnitude < rad*rad);
    }
}
