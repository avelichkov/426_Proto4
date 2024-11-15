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

    public ScreenShake Camera;

    public enum Stats
    {
        RELOAD_TIME, MOVEMENT_SPEED, DAMAGE, SPAWN_RATE
    }

    //1:ReloadTime, 
    public static int[] levels;
    public static float[] stats;
    public static int TotalKills;
    public static int KillsTillNextLevel;
    public static int CurrentKills;

    //All the UI Stuff
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI[] _stats;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _nextLevelText; 
    private float _timer = 120f;
    private bool _endTriggered = false;

    //Game feel stuff
    public PlayerEffects playereff;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
        levels = new int[] { 1, 1, 1, 1 };
        stats = new float[] { 1f, 1f, 1f, 0.7f };
    }

    void Start()
    {
        AudioManager.instance.Play("Music");
        Time.timeScale = 1.0f;
        TotalKills = 0;
        CurrentKills = 0;
        KillsTillNextLevel = 3;
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

        int keyUpgrade = GetNumKeyDown();
        if (keyUpgrade != -1)
        {
            PlusStat(keyUpgrade-1);
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0 && !_endTriggered)
        {
            TimeOut();
        }
        float DisplayVal = Mathf.Round(_timer * 10) / 10;
        _timerText.text = "Time: " + DisplayVal;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        _endTriggered = false;
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
        if (levels[index] != 4 && CurrentKills >= KillsTillNextLevel)
        {
            AudioManager.instance.Play("Upgrade");
            levels[index]++;
            CurrentKills -= KillsTillNextLevel;
            KillsTillNextLevel = (int)(KillsTillNextLevel * 1.5f);
            if (CurrentKills <= KillsTillNextLevel) _nextLevelText.color = Color.white;
            _nextLevelText.text = CurrentKills + "/" + KillsTillNextLevel;
            if (index == 0 || index == 3)
            {
                Debug.Log(index);
                Debug.Log(stats[index]);
                stats[index] *= 0.5f;
            }
            else if (index == 2)
            {
                stats[index] += 1f;
            }
            else
            {
                stats[index] *= 2.0f;
                Debug.Log(index);
                Debug.Log(stats[index]);
            }
            _stats[index].text = GetBar(levels[index]);
        }
    }

    public void ZombieKilled()
    {
        AudioManager.instance.Play("Kill");
        TotalKills++;
        CurrentKills++;
        if (CurrentKills >= KillsTillNextLevel) _nextLevelText.color = Color.green;
        _nextLevelText.text = CurrentKills + "/" + KillsTillNextLevel;
        _score.text = TotalKills.ToString();
    }

    public void GameOver()
    {
        AudioManager.instance.Stop("Music");
        AudioManager.instance.Play("Game Over");
        playereff.DeathEffects();
        _score.color = Color.red;
        _score.text = "You Lose, you get nothing, good day sir.";
        StartCoroutine(GameOverEnum());
    }

    private IEnumerator GameOverEnum()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(4f); //changed this to realtime so that it works at timescale 0
        RestartGame();

    }


    public void TimeOut()
    {
        AudioManager.instance.Stop("Music");
        AudioManager.instance.Play("Victory");
        Time.timeScale = 0.0f;
        _timerText.color = Color.green;
        //Camera.TriggerShake(2f);
        _endTriggered = true;
        _score.text = "Kills: " + TotalKills;
        if (TotalKills < 500)
        {
            _score.text += " | 1/3 Stars";
        }
        else if (TotalKills < 750)
        {
            _score.text += " | 2/3 Stars";
        }
        else
        {
            _score.text += " | 3/3 Stars!!!!";
        }
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

    private int GetNumKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
        return -1;
    }
}

