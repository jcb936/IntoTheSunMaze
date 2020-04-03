using UnityEngine;
using UnityEngine.UI;
using AlgoConstants;

// An implementation of GameMenu for the test menu. It adds the possibility to generate a new maze of desired size.
public class TestMenu : GameMenu {
    
    public Button newMazeButton;

    public Slider heightSlider;

    public Slider widthSlider;

    public Text heightSliderInfoText;

    public Text widthSliderInfoText;

    private int mazeHeight;

    private int mazeWidth;

    public static TestMenu instance;

    private new void Awake() {
        if (instance == null)
            instance = this;
        mazeHeight = (int)heightSlider.value;
        mazeWidth = (int)widthSlider.value;
        newMazeButton.onClick.AddListener(() => GameManager.instance.NewMaze(mazeHeight, mazeWidth, Constants.PRIM_ALGO));
        heightSlider.onValueChanged.AddListener(UpdateHeight);
        widthSlider.onValueChanged.AddListener(UpdateWidth);
        heightSliderInfoText.text = "Height: " + mazeHeight;
        widthSliderInfoText.text = "Width: " + mazeWidth;
        base.Awake();
    }

    private void UpdateHeight(float value) {
        mazeHeight = (int)value;
        heightSliderInfoText.text = "Height: " + mazeHeight;
    }

    private void UpdateWidth(float value) {
        mazeWidth = (int)value;
        widthSliderInfoText.text = "Width: " + mazeWidth;
    }
}