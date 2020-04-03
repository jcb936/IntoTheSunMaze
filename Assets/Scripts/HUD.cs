using UnityEngine;
using UnityEngine.UI;

// Script that manages the HUD. This script is also responsible for
// the timer operations of the game.

public class HUD : MonoBehaviour {

    // Make it a singleton.
    
    public static HUD instance;

    // UI Elements.

    [SerializeField] private Text timerText = null;

    [SerializeField] private Text roomText = null;

    public int roomNumber { get { return roomNumber; } set { roomText.text = "ROOM " + value; } }

    public float timer { get; set; }

    private bool timerActive;

    // This bool is true if the active camera is the player one.

    public bool playerCameraActive { get; set; }

    private void Awake() {
        if (instance == null)
            instance = this;
        timerActive = false;
        roomNumber = 1;
        playerCameraActive = false;
    }
    public void StartTimer() {
        timerActive = true;
    }

    // Manages timer and game over. If player camera is active,
    // time runs more slowly.

    private void Update() {
        if (timerActive && timer > 0) {
            if (!playerCameraActive) {
                timer -= Time.deltaTime;
                timerText.GetComponent<Animator>().speed = 1;
            } else { 
                timer -= Time.deltaTime*0.75f;
                timerText.GetComponent<Animator>().speed = 0.75f;
            }
            timerText.text = ((int)timer).ToString();
        } else if ( timerActive && timer <= 0 )
            GameManager.instance.GameOver();
    }

    public void DisableHUD() {
        timerText.enabled = false;
        roomText.enabled = false;
    
    }
}