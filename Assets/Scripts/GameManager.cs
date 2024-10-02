using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

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

    //All the UI Stuff
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI[] _stats;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _nextLevelText; 
    private float _timer = 120;

    void Awake() {
        CurrentKills = 100;
        ZombieKilled();
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
        _timerText.text = "Time: " + DisplayVal;
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

    public void PlusStat(int index)
    {
        if (levels[index] != 4 && CurrentKills > KillsTillNextLevel)
        {
            levels[index]++;
            stats[index] = 0;
            //AudioManager.instance.Play([statupnoise]);
            CurrentKills -= KillsTillNextLevel;
            KillsTillNextLevel+=1;
            _nextLevelText.text = CurrentKills + "/" + KillsTillNextLevel;
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
            _stats[index].text = GetBar(levels[index]);
        }
    }

    public void ZombieKilled()
    {
        TotalKills++;
        CurrentKills++;
        _nextLevelText.text = CurrentKills + "/" + KillsTillNextLevel;
        _score.text = TotalKills.ToString();
    }

    public static void GameOver()
    {
        
    }

    public static void TimeOut()
    {

    }

    private String GetBar(int level)
    {
        switch (level)
        {
            case 1:
                return "[ ■ ] [ - ] [ - ] [ - ]";
            case 2:
                return "[ ■ ] [ ■ ] [ - ] [ - ]";
            case 3: 
                return "[ ■ ] [ ■ ] [ ■ ] [ - ]";
            case 4: 
                return "[ ■ ] [ ■ ] [ ■ ] [ ■ ]";
            default:
                return "[ ■ ] [ - ] [ - ] [ - ]";
        }
    }
}

