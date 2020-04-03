using UnityEngine;
using UnityEngine.UI;
using AlgoConstants;

// Start menu script

public class StartMenu : MonoBehaviour 
{

    public Button startButton;

    public Button mazeTestButton;

    public Button quitButton;

    public GameObject tutorialScreen;

    private void Awake() 
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(() => Application.Quit());
        mazeTestButton.onClick.AddListener(() => GameManager.instance.NewMaze(15, 15, Constants.PRIM_ALGO));
    } 

    private void StartGame() 
    {
        tutorialScreen.SetActive(true);
        Destroy(gameObject);
    }


}