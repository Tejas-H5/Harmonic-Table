using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInvalidateable{
    void Invalidate();
}

public class UIInvalidator : MonoBehaviour , IInvalidateable{
    public Transform[] objects;

    public void Invalidate(){
        foreach(var obj in objects){
            obj.GetComponent<IInvalidateable>().Invalidate();
        }
    }
}
