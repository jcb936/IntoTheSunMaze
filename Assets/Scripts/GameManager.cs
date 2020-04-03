using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AlgoConstants;
using System.Collections;
using UnityEngine.EventSystems;

// This namespace was made to define algorithms name as constants; 
// hopefully it will be extended in a future update of the project to include
// more algorithms.

namespace AlgoConstants {
    public static class Constants {
        public const int PRIM_ALGO = 0;
    }
}

// Main class that manages the logic of the game. The object attached to the script is generated
// in the start scene and kept through all loadings.

public class GameManager : MonoBehaviour {

    // Make it a singleton

    public static GameManager instance;

    // Prefabs

    public GameObject mazeMaker;

    public GameObject player;

    public GameObject objective;

    public GameObject timeExtension;

    // Properties

    public PlayerControl playerInstance { get; set; }

    public AudioSource audioSource { get; set; }

    // Other variables

    private Light levelLight; // Main light of the scene

    private CinemachineVirtualCamera vmcamBirdView; // Birdview camera

    private CinemachineVirtualCamera vmcamPlayerView; // Player camera

    private float levelTime; // Initial timer time for the level

    private int mazeWidth;

    private int mazeHeight;

    private int mazeAlgo;

    private bool testMode; // If true doesn't spawn HUD and makes Test Menu for custom maze generation available

    private int currentLevel;

    private int extensionsCount; // Number of time extensions to spawn for the level

    private void Awake() {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        testMode = true;
        levelTime = 10;
        DontDestroyOnLoad(this);
        mazeHeight = 0;
        mazeWidth = 0;
        mazeAlgo = Constants.PRIM_ALGO;
        currentLevel = 1;
        extensionsCount = 0;
        audioSource = GetComponent<AudioSource>();
    }

    // Switch between player camera and birdview camera. When switching it
    // also modify the pitch of the clip to slow it down, together with the timer
    // by modifying playerCameraActive in HUD

    private void ChangeCamera() {

        if (vmcamBirdView.enabled) {
            vmcamBirdView.enabled = false;
            vmcamPlayerView.enabled = true;
            HUD.instance.playerCameraActive = true;
            audioSource.pitch *= 0.75f;
        }
        else {
            vmcamPlayerView.enabled = false;
            vmcamBirdView.enabled = true;
            HUD.instance.playerCameraActive = false;
            audioSource.pitch *= 4f/3f;
        }
    }

    private void Update() {

        if (vmcamBirdView != null && Input.GetKeyDown(KeyCode.Q))
            ChangeCamera();

        if (PauseMenu.instance != null && Input.GetKeyDown(KeyCode.Escape)) {
            if (!testMode) {
                if (!PauseMenu.instance.isActiveAndEnabled)
                    PauseGame();
                else
                    ContinueGame();
            }
            else if (testMode) {
                if (TestMenu.instance != null && !TestMenu.instance.isActiveAndEnabled)
                    PauseGame();
                else
                    ContinueGame();
            }
        }

        if (!testMode && levelLight != null && levelLight.intensity < 1)  // Increase light intensity
            levelLight.intensity += Time.deltaTime / levelTime;

        if (audioSource.isPlaying)  // Increase the volume
            audioSource.volume += Time.deltaTime / levelTime;
    }

    // Continue game after you pause it

    private void ContinueGame() {

        Time.timeScale = 1;
        playerInstance.enabled = true;
        if (!testMode) {
            audioSource.Play();
            PauseMenu.instance.gameObject.SetActive(false);
        } else
            TestMenu.instance.gameObject.SetActive(false);
    }

    // Pause the game

    private void PauseGame() {

        Time.timeScale = 0;
        playerInstance.enabled = false;
        audioSource.Pause();
        if (!testMode)
            PauseMenu.instance.gameObject.SetActive(true);
        else
            TestMenu.instance.gameObject.SetActive(true);
    }


    public void NewGame() {
        testMode = false;
        mazeHeight = 10;
        mazeWidth = 10;
        extensionsCount = 5;
        StartCoroutine(PrepareMaze());
    }

    // This is used to generate a new maze from the test menu when in TestMode

    public void NewMaze(int mazeHeight, int mazeWidth, int mazeAlgorithm) {
        testMode = true;
        this.mazeHeight = mazeHeight;
        this.mazeWidth = mazeWidth;
        this.mazeAlgo = mazeAlgorithm;
        StartCoroutine(PrepareMaze());
    }

    // Adapt the pitch to the time for the level and start the clip

    private void PlayLevelClip() {
        audioSource.Stop();
        audioSource.volume = 0;
        audioSource.pitch = (6.5f / levelTime);
        audioSource.Play();
    }

    // Extend the time when collect extensions and adjust audioclip playback time and light intensity

    public void Extension() {
        float playbackTime = audioSource.time;
        float volume = audioSource.volume;
        playbackTime = Mathf.Clamp(playbackTime - 1, 0, 100);
        volume = Mathf.Clamp(volume - Time.deltaTime / levelTime, 0, 100);
        audioSource.time = playbackTime;
        audioSource.volume = volume;

        levelLight.intensity -= 1f / levelTime;
    }

    public void NextLevel() {
        mazeHeight += 2;
        mazeWidth += 2;
        currentLevel++;
        extensionsCount = (int)mazeHeight * mazeWidth / 20; // Function for extension spawn number

        levelTime = currentLevel < 4 ? levelTime + 5 : (currentLevel < 9 ? levelTime + 2 : levelTime + 1); // How time changes with level
        StartCoroutine(PrepareMaze());
    }

    public void GameOver() {
        audioSource.Stop();
        playerInstance.enabled = false;
        PauseMenu.instance.gameObject.SetActive(false);
        GameOverScreen.instance.gameObject.SetActive(true);
        GameOverScreen.instance.roomPassed = currentLevel;
    }

    // After loading the scene, get the objects of the scene

    private void GetMainSceneObjects() {
        vmcamBirdView = GameObject.FindGameObjectWithTag("BirdCamera").GetComponent<CinemachineVirtualCamera>();
        vmcamPlayerView = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        levelLight = GameObject.FindGameObjectWithTag("LevelLight").GetComponent<Light>();
    }

    // Maze and instances generation coroutine

    private IEnumerator PrepareMaze() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
            yield return null;
        
        GetMainSceneObjects();

        if (MazeMaker.instance == null)
            Instantiate(mazeMaker, Vector3.zero, Quaternion.identity);

        MazeMaker.instance.GenerateMaze(mazeHeight, mazeWidth, mazeAlgo);
        GameObject instanceOfPlayer = Instantiate(player, Vector3.zero, Quaternion.identity);
        playerInstance = instanceOfPlayer.GetComponent<PlayerControl>();
        vmcamBirdView.enabled = false;
        vmcamPlayerView.enabled = false;
        vmcamPlayerView.LookAt = playerInstance.transform;
        vmcamPlayerView.Follow = playerInstance.transform;
        AdjustCamera(mazeHeight, mazeWidth);
        vmcamBirdView.enabled = true;


        PauseMenu.instance.gameObject.SetActive(false);
        TestMenu.instance.gameObject.SetActive(false);
        GameOverScreen.instance.gameObject.SetActive(false);
        HUD.instance.playerCameraActive = false;


        if (!testMode)
        {
            Instantiate(objective, new Vector3(mazeWidth - 1, mazeHeight - 1.25f, 0), Quaternion.identity);
            EventSystem.current.SetSelectedGameObject(PauseMenu.instance.applyChangesButton.gameObject);
            SpawnExtensions();
            StartHUD();
            PlayLevelClip();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(TestMenu.instance.applyChangesButton.gameObject);
            HUD.instance.DisableHUD();
            levelLight.intensity = 1;
        }

        yield return null; 

        if (playerInstance != null && !playerInstance.CheckIfVisible())
            AdjustCameraHorizontal();

    }

    private void StartHUD() {
        HUD.instance.timer = levelTime;
        HUD.instance.roomNumber = currentLevel;
        HUD.instance.StartTimer();
    }

    // This is the general camera adjustment with respect to the height of the maze. 
    // This is enough in most of the cases. 

    private void AdjustCamera(int mazeHeight, int mazeWidth) {
        vmcamPlayerView.m_Lens.OrthographicSize = 2.5f;
        vmcamBirdView.m_Lens.OrthographicSize = mazeHeight * 0.5f;
        vmcamBirdView.transform.position = new Vector3(mazeWidth * 0.5f - 0.5f, mazeHeight * 0.5f - 0.5f, -10);
    }

    // If changing the resolution has hidden the player, or you generate a maze
    // more wide than high, then this fixes the camera by scaling it with respect to width and aspect ratio.

    public void AdjustCameraHorizontal() {
        vmcamBirdView.m_Lens.OrthographicSize = (mazeWidth + 0.1f) * Screen.height / Screen.width * 0.5f;
    }

    private void SpawnExtensions() {
        for (int i = 0; i < extensionsCount; i++)
            Instantiate(timeExtension, new Vector3(Random.Range(1, mazeWidth - 1), Random.Range(1, mazeHeight - 1) - 0.25f, 0), Quaternion.identity);
    }
}