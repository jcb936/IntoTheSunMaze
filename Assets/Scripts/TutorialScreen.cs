using UnityEngine;
using UnityEngine.SceneManagement;

// Tutorial screen that appears before starting the game

public class TutorialScreen : MonoBehaviour {
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.instance.NewGame();
        else if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }
}