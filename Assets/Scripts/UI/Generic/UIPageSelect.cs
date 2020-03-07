using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this can probably be used for other things as well
public class UIPageSelect : MonoBehaviour{
    public GameObject[] pages;
    int _selIndex = 0;

    public void Start(){
        for(int i = 0; i < pages.Length; i++){
            pages[i].SetActive(i==_selIndex);
        }
    }

    public void SelectPage(int i){
        pages[_selIndex].SetActive(false);
        pages[i].SetActive(true);
        _selIndex = i;
    }
}
