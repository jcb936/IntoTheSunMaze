using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This script is used to make the dropdowns menu autoscroll with the keyboard, and 
// is attached directly to the dropdown UI elements. It is based on this
// script: https://www.alexandrow.org/blog/autoscroll-dropdowns-in-unity/

public class EventSensitiveScrollRect : MonoBehaviour, IUpdateSelectedHandler {

    // How much to "overshoot" when scrolling, relative to the selected item's height
    private static float SCROLL_MARGIN = 0.3f; 

    private ScrollRect sr;

    public void Awake() {
        sr = this.gameObject.GetComponent<ScrollRect>();
    }

    public void OnUpdateSelected(BaseEventData eventData) {
        // Helper variables
        float contentHeight = sr.content.rect.height;
        float viewportHeight = sr.viewport.rect.height;

        // What bounds must be visible?
        float centerLine = eventData.selectedObject.transform.localPosition.y; // Selected item's center
        float upperBound = centerLine + (eventData.selectedObject.GetComponent<RectTransform>().rect.height / 2f); // Selected item's upper bound
        float lowerBound = centerLine - (eventData.selectedObject.GetComponent<RectTransform>().rect.height / 2f); // Selected item's lower bound

        // What are the bounds of the currently visible area?
        float lowerVisible = (contentHeight - viewportHeight) * sr.normalizedPosition.y - contentHeight;
        float upperVisible = lowerVisible + viewportHeight;

        // Is our item visible right now?
        float desiredLowerBound;
        if (upperBound > upperVisible) {
            // Need to scroll up to upperBound
            desiredLowerBound = upperBound - viewportHeight + eventData.selectedObject.GetComponent<RectTransform>().rect.height * SCROLL_MARGIN;
        }
        else if (lowerBound < lowerVisible) {
            // Need to scroll down to lowerBound
            desiredLowerBound = lowerBound - eventData.selectedObject.GetComponent<RectTransform>().rect.height * SCROLL_MARGIN;
        }
        else {
            // Item already visible - all good
            return;
        }

        // Normalize and set the desired viewport
        float normalizedDesired = (desiredLowerBound + contentHeight) / (contentHeight - viewportHeight);
        sr.normalizedPosition = new Vector2(0f, Mathf.Clamp01(normalizedDesired));
    }

}