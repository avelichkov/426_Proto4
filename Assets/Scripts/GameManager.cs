using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum Stats
    {
        RELOAD_TIME = 1,DAMAGE = 2,MOVEMENT_SPEED = 3, SPAWN_RATE = 4
    }   

    //1:ReloadTime, 
    public static int[] levels;
    public static float[] stats;
    public static float ReloadTime;
    public static float  Damage;
    public static float MovementSpeed;
    public static float SpawnRate;

    void Awake() {
        if (instance == null) 
        {
            instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        levels = new int[]{1,1,1,1};
        stats = new float[]{0.5f,1f,1f,1f};
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        // For debugging purposes in the editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

