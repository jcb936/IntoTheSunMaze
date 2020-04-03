using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Script that manages the game over screen.

public class GameOverScreen : MonoBehaviour {

    // Make it a singleton
    public static GameOverScreen instance;

    // Final score text (numbers of room passed)

    [SerializeField] private Text roomPassedText = null;

    public int roomPassed { 
        get { 
            return roomPassed; 
        } set { 
            roomPassedText.text = "You survived for " + value + " rooms."; 
        } 
    }

    private void Awake() {
        if (instance == null) 
            instance = this;
    }

    // Go back to main menu

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0);
    }
}