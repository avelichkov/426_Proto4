using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum Stats
    {
        RELOAD_TIME,DAMAGE,MOVEMENT_SPEED, SPAWN_RATE
    }   

    //1:ReloadTime, 
    public static int[] levels;
    public static float[] stats;
    public static int TotalKills;
    public static int KillsTillNextLevel = 2;
    public static int CurrentKills = 0;

    private float _timer = 120;

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
        _timer -= Time.deltaTime;
        float DisplayVal = Mathf.Round(_timer * 10) / 10;
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
    public void PlusReload(){PlusStat(0);}
    public void PlusDamage(){PlusStat(1);}
    public void PlusSpeed(){PlusStat(2);}
    public void PlusSpawnRate(){PlusStat(3);}

    private void PlusStat(int index)
    {
        if (levels[index] != 4 && CurrentKills > KillsTillNextLevel)
        {
            levels[index]++;
            stats[index] = 0;
            //AudioManager.instance.Play([statupnoise]);
            CurrentKills -= KillsTillNextLevel;
            KillsTillNextLevel+=1;
            if ( index == 0)
            {
                stats[index] *= 0.5f;
            }
            if (index == 1)
            {
                stats[index] +=1;
            }
            else
            {
                stats[index] *= 2;
            }
        }
    }

    public static void ZombieKilled()
    {
        TotalKills++;
        CurrentKills++;
    }

    public static void GameOver()
    {
        
    }

    public static void TimeOut()
    {

    }
}

