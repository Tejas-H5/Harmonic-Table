using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollbarInput : MonoBehaviour ,
                IDragHandler,
                IPointerDownHandler {
    
    public bool vertical;
    public bool flip;
    float _val;

    public UnityEvent onValueChange;

    public float Value {
        get=>_val;
        set{
            SetAmount((value-min)/(max-min));
        }
    }

    //Any number above zero turns this continuous slider into a discrete one
    public int subdivisions = 0;
    public float min = 0;
    public float max = 1;

    public RectTransform scrollBar;
    public Text textReadout;
    
    //needs to be a child of this transform
    public RectTransform fillRect;

    Vector3[] _corners = new Vector3[4];

    //set to a positive value to restrict scrolling beyond a certain point (0-1 ofc)
    public float scrollUpperbound = -1;

    public void OnPointerDown(PointerEventData pointerEventData){
        OnDrag(pointerEventData);
    }

    public void UpdateAppearance(float amount){
        updateAppearanceNormalized((amount-min)/(max-min));
    }

    //amount is between 0 and 1
    void updateAppearanceNormalized(float amount){
        var rtf = (transform as RectTransform);

        textReadout.text = _val.ToString();

        if(vertical){
            float y = amount*rtf.rect.height;
            scrollBar.anchoredPosition = new Vector2(scrollBar.anchoredPosition.x,y);
            fillRect.sizeDelta = new Vector2(fillRect.sizeDelta.x, y);

        } else {
            float x = amount*rtf.rect.width;
            scrollBar.anchoredPosition = new Vector2(x, scrollBar.anchoredPosition.y);
            fillRect.sizeDelta = new Vector2(x, fillRect.sizeDelta.y);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        var rtf = (transform as RectTransform);
        rtf.GetWorldCorners(_corners);

        float x0, x1;
        if(vertical){
            x0 = _corners[1].y;
            x1 = _corners[0].y;
        } else {
            x0 = _corners[1].x;
            x1 = _corners[2].x;
        }

        if(flip){
            float temp = x0;
            x0=x1;
            x1=temp;
        }

        Vector3 pos = data.position;
        float m;
        if(vertical){
            m = pos.y;
        } else {
            m = pos.x;
        }
        
        float amount = (m-x0)/(x1-x0);

        SetAmount(amount);
    }

    public void SetValue(float v){
        SetValuePassive(v);
        onValueChange.Invoke();
    }

    public void SetValuePassive(float v){
        _val = v;
        UpdateAppearance(_val);
    }

    public void SetAmount(float amount){
        amount = Mathf.Clamp01(amount);

        if(subdivisions > 0){
            amount = Mathf.Round(amount * subdivisions)/(float)(subdivisions);
            print(amount);
        }

        SetValue(min + (amount * (max-min)));
    }
}
