using UnityEngine;

// Simple DontDestroyOnLoad script to attach to gameobjects with no other scripts.

public class DontDestroy : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(this);
    }
}
