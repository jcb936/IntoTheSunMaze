using UnityEngine;

// Simple manager for screen resolutions.

public class ScreenManager : MonoBehaviour{

    public static ScreenManager instance;

    public Resolution[] availableResolutions { get; set; }
    
    private int resCycle;
    
    private int resArrayLength;
    
    private Resolution maxRes;

    private void Awake() {
        if (instance == null)
            instance = this;

        availableResolutions = Screen.resolutions;
        resArrayLength = availableResolutions.Length;
        resCycle = resArrayLength - 1;
        maxRes = availableResolutions[resCycle];
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resCycle++;
            resCycle %= resArrayLength - 1;
            Screen.SetResolution(availableResolutions[resCycle].width, availableResolutions[resCycle].height, false);
            if (!GameManager.instance.playerInstance.CheckIfVisible())
                GameManager.instance.AdjustCameraHorizontal();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!Screen.fullScreen)
                Screen.SetResolution(maxRes.width, maxRes.height, true);
            else
                Screen.SetResolution(availableResolutions[resCycle % (resArrayLength - 1)].width, availableResolutions[resCycle % (resArrayLength - 1)].height, false);

            if (!GameManager.instance.playerInstance.CheckIfVisible())
                GameManager.instance.AdjustCameraHorizontal(); // After changing resolution, if player is not visible then resize camera horizontally.
        }
    }

    public void SetResolution(int width, int height, bool fullscreen) {
        Screen.SetResolution(width, height, fullscreen);
    }
}
