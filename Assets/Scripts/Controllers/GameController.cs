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
    private int levelIndex = 0;


    private bool loaded = false;
    private int enemiesRemaining = 0;


    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
            LoadLevel();
        }
    }

    void Update() {
        if (loaded && enemiesRemaining <= 0) { 
            NextLevel();
        }
    }

    public static GameController GetInstance() {
        return instance;
    }

    public static void LoadLevel() {
        instance.sceneState = SCENE_STATE.GAME;
        instance.enemiesRemaining = 0;
        instance.loaded = false;
        SceneManager.LoadScene("game");
    }

    public static int GetLevelIndex() {
        return instance.levelIndex;
    }

    public static void GameOver() {
        NewGame();
    }

    public static void NewGame() {
        instance.levelIndex = 0;
        LoadLevel();
    }

    public static void NextLevel() {
        instance.levelIndex++;
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
