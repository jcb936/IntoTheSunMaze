using UnityEngine;
using UnityEngine.EventSystems;

// This class starts and stops the cursor sprite animation when selected and deselected
// in UI elements.

public class ButtonSelectHandler : MonoBehaviour, ISelectHandler, IDeselectHandler {

    // 
    public void OnSelect(BaseEventData data) {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData data) {
        transform.GetChild(1).gameObject.SetActive(false);
    }
}