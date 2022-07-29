using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameView gameView;
    public MenuView menuView;

    public LevelEndView levelEndView;
    public GameObject completedView;
    public GameObject failedView;

    public bool isGameStarted;

    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
    }

    void Start()
    {
        InitGame();
    }

    void InitGame()
    {
        isGameStarted = false;

        DestroyObjects();

        DebugManager.instance.Init();

        InitCanvas();

        LevelManager.instance.InitLevel();
        EnviromentManager.instance.InitEnviroment();
        CharacterManager.instance.Initialize();
        CrowdManager.instance.Initialize();
        CameraController.instance.InitCamera();
    }

    void InitCanvas()
    {
        menuView.gameObject.SetActive(true);
        gameView.gameObject.SetActive(false);

        levelEndView.gameObject.SetActive(false);
        completedView.SetActive(false);
        failedView.SetActive(false);
    }

    public void StartGame()
    {
        isGameStarted = true;

        menuView.gameObject.SetActive(false);
        gameView.gameObject.SetActive(true);

        CrowdManager.instance.StartBattle();

        //TinySauce.OnGameStarted(DataManager.instance.GetLevelNumber());
    }

    public void LevelCompleted()
    {
        if (!isGameStarted) return;

        isGameStarted = false;

        //TinySauce.OnGameFinished(true, 0, DataManager.instance.GetLevelNumber());

        LevelManager.instance.LevelCompleted();

        menuView.gameObject.SetActive(false);
        gameView.gameObject.SetActive(false);

        levelEndView.gameObject.SetActive(true);
        completedView.SetActive(true);
        failedView.SetActive(false);
    }

    public void LevelFailed()
    {
        if (!isGameStarted) return;

        isGameStarted = false;

        //TinySauce.OnGameFinished(false, 0, DataManager.instance.GetLevelNumber());

        LevelManager.instance.LevelFailed();

        menuView.gameObject.SetActive(false);
        gameView.gameObject.SetActive(false);

        levelEndView.gameObject.SetActive(true);
        completedView.SetActive(false);
        failedView.SetActive(true);
    }

    public void RestartGame()
    {
        InitGame();
    }

    public void DoHaptic()
    {
        MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
    }

    public void DoSuccessHaptic()
    {
        MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
    }

    public void ToggleDebugManager()
    {
        DebugManager.instance.gameObject.SetActive(!DebugManager.instance.gameObject.activeSelf);
    }

    void DestroyObjects()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("destroyOnInit");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Break();
        }
        else if (Input.GetKeyDown("r"))
        {
            RestartGame();
        }
    }
#endif
}
