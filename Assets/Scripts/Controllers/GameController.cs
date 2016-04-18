using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public enum SCENE_STATE {
        START,
        TUTORIAL,
        GAME
    }

    public static bool PROFILER_ENABLED = false;
    public static GameController instance;

    private SCENE_STATE sceneState = SCENE_STATE.GAME;
    private int numLevels = 3;
    private int level = 0;

    private bool loaded = false;
    private int enemiesRemaining = 0;

    private float transitionTime = 1.5f;
    private float transitionTimer = 0;
    private bool levelComplete = false;
    private bool gameOver = false;

    private bool onGameOverScreen = false;


    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (instance.onGameOverScreen) {
            if (Input.anyKeyDown) {
                LoadLevel();
            }
        } else {
            if (loaded && enemiesRemaining <= 0 && !instance.levelComplete && !gameOver) {
                SoundController.PlayLevelComplete();
                instance.levelComplete = true;
            } else if (instance.levelComplete) {
                if (instance.transitionTimer > instance.transitionTime) {
                    instance.transitionTimer = 0;
                    NextLevel();
                } else {
                    instance.transitionTimer += Time.deltaTime;
                }
            } else if (instance.gameOver) {
                if (instance.transitionTimer > instance.transitionTime) {
                    instance.transitionTimer = 0;
                    if (GetLevelIndex() > 0) {
                        instance.level = 1;
                    } else {
                        instance.level = 0;
                    }
                    instance.gameOver = false;
                    LoadEndGameScreen();
                } else {
                    instance.transitionTimer += Time.deltaTime;
                }
            }
        }
    }

    public static GameController GetInstance() {
        return instance;
    }

    public static void LoadLevel() {
        instance.onGameOverScreen = false;
        instance.sceneState = SCENE_STATE.GAME;
        instance.enemiesRemaining = 0;
        instance.loaded = false;
        instance.levelComplete = false;
        SceneManager.LoadScene("game");
    }

    public static void LoadEndGameScreen() {
        instance.onGameOverScreen = true;
        SceneManager.LoadScene("gameOver");
    }

    public static int GetLevelIndex() {
        if (instance.level == 0) return 0;
        int levelIndex = instance.level % instance.numLevels;
        if (levelIndex == 0) levelIndex = instance.numLevels;
        return levelIndex;
    }

    public static int GetDifficulty() {
        return (int) (instance.level / instance.numLevels);
    }

    public static int GetNumLevels() {
        return instance.numLevels;
    }

    public static int GetRawLevel() {
        return instance.level;
    }

    public static void GameOver() {
        instance.gameOver = true;
    }

    public static void NextLevel() {
        instance.level++;
        LoadLevel();
    }

    public static void IncrementEnemies() {
        instance.enemiesRemaining++;
    }

    public static void DecrementEnemies() {
        instance.enemiesRemaining--;
    }

    public static void DoneLoading() {
        instance.loaded = true;
    }
}
