using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Abstract class which is implemented in the Pause Menu and the Test Menu.
public abstract class GameMenu : MonoBehaviour {

    // UI elements. Public variables are serialized in the inspector.

    public Button applyChangesButton;

    public Dropdown resolutionDropdown;

    public Toggle fullscreenToggle;

    public Button mainMenuButton;

    // Buttons are initialised with function listeners, and resolution dropdown
    // is initialised with the list of resolutions.

    protected virtual void Awake() {
        mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        applyChangesButton.onClick.AddListener(ApplyChanges);
        List<Dropdown.OptionData> resolutionList = new List<Dropdown.OptionData>();
        Resolution[] availableResolutions = ScreenManager.instance.availableResolutions;
        Resolution currentRes = Screen.currentResolution;
        int currentResIndex = 0;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            resolutionList.Add(new Dropdown.OptionData(availableResolutions[i].width + "x" + availableResolutions[i].height));
            if (availableResolutions[i].width == currentRes.width && availableResolutions[i].height == currentRes.height)
                currentResIndex = i;
        }
        resolutionDropdown.options = resolutionList;
        resolutionDropdown.value = currentResIndex;
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    // Apply new resolution.

    protected virtual void ApplyChanges() {
        string resToApply = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolution = resToApply.Split(new char[] { 'x' });
        ScreenManager.instance.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), fullscreenToggle.isOn);
    }

    // This is to make sure time starts again when reloading the scene from the test menu.

    protected virtual void OnDestroy() {
        Time.timeScale = 1;
    }
}