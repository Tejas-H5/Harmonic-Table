using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : EventTrigger {

    private bool _dragging;

    public void Update() {
        if (_dragging) {
            var par = transform.parent;
            var trf = (par as RectTransform);
            var halfW = trf.rect.width/2.0f;
            var halfH = trf.rect.height/2.0f;
            var lowerX = par.position.x;
            var upperX = par.position.x + halfW;
            var lowerY = par.position.y - halfH;
            var upperY = par.position.y + halfH;

            transform.position = new Vector2(
                    Mathf.Clamp(Input.mousePosition.x,lowerX, upperX),
                    Mathf.Clamp(Input.mousePosition.y,lowerY, upperY)
                );
        }
    }

    public override void OnPointerDown(PointerEventData eventData) {
        _dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData) {
        _dragging = false;
    }
}