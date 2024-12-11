using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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
    [SerializeField] private GameObject _upgrades;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI[] _statsDisplay;
    [SerializeField] private TextMeshProUGUI _timerText;
    //[SerializeField] private TextMeshProUGUI _nextLevelText;
    private float _timer = 120f;
    public bool _endTriggered = false;
    private PlayerMove player;

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
        stats = new float[] { 1f, 1.5f, 1f, 0.7f };
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform.GetComponent<PlayerMove>();
        if (GameObject.FindWithTag("Player") == null) Debug.Log("no player obj");
        if (player == null) Debug.Log("no player");
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

        // int keyUpgrade = GetNumKeyDown();
        // if (keyUpgrade != -1)
        // {
        //     PlusStat(keyUpgrade - 1);
        // }

        _timer -= Time.deltaTime;
        if (_timer <= 0 && !_endTriggered)
        {
            TimeOut();
        }
        float DisplayVal = Mathf.Round(_timer * 10) / 10;
        _timerText.text = /*"Time: " + */DisplayVal.ToString();
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
        Debug.Log("increased stat");
        if (CurrentKills >= KillsTillNextLevel)
        {
            AudioManager.instance.Play("Upgrade");
            levels[index]++;
            CurrentKills -= KillsTillNextLevel;
            KillsTillNextLevel = (int)(KillsTillNextLevel * 1.2f + 0.4f);
            player.UpdateColor(0f);
            //if (CurrentKills <= KillsTillNextLevel) _nextLevelText.color = Color.white;
            //_nextLevelText.text = CurrentKills + "/" + KillsTillNextLevel

            if (index == 0){
                stats[index] *= 0.6f;
            } else if (index == 1) {
                stats[index] *= 1.75f;
            } else if (index == 2) {
                stats[index] *= 2f;
            } else {
                stats[index] *= 0.5f;
            }

            //_stats[index].text = GetBar(levels[index]);
        }
        ToggleUpgrades(false);
    }

    public void ZombieKilled()
    {
        AudioManager.instance.Play("Kill");
    }
    public void WispCollected(int count = 1) // Default to 1 point
    {
        AudioManager.instance.Play("Collect");
        TotalKills += count; // Increment total kills by the given count
        if (CurrentKills < KillsTillNextLevel)
        {
            CurrentKills += count; // Increment current kills by the given count
        }
        player.UpdateColor((float)CurrentKills / KillsTillNextLevel);
        if (CurrentKills >= KillsTillNextLevel)
        {
            ToggleUpgrades(true); // Trigger upgrades if necessary
        }
        _score.text = TotalKills.ToString(); // Update the UI score
    }


    public void GameOver()
    {
        //AudioManager.instance.Stop("Music");
        //AudioManager.instance.Play("Game Over");
        //playereff.DeathEffects();
        //_score.color = Color.red;
        //_score.text = "You Lose, you get nothing, good day sir.";
        player.GetComponent<SpriteRenderer>().color = Color.red;
        AudioManager.instance.Play("Death");
        StartCoroutine(TimeOutCR());
    }

    public void ToggleUpgrades(bool active)
    {
        _upgrades.SetActive(active);
    }

    private IEnumerator GameOverEnum()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(4f); //changed this to realtime so that it works at timescale 0
        //RestartGame();

    }

    private IEnumerator TimeOutCR()
    {
        AudioManager.instance.Stop("Music");
        Time.timeScale = 0.0f;
        _endTriggered = true;
        yield return new WaitForSecondsRealtime(0.75f);
        _endScreen.SetActive(true);
        //200, 500, 900, 
        if (TotalKills < 150)
        {
            GameObject.Find("Star1").SetActive(false);
            GameObject.Find("Star2").SetActive(false);
            GameObject.Find("Star3").SetActive(false);
            AudioManager.instance.Play("Victory0");
        }
        else if (TotalKills < 250)
        {
            GameObject.Find("Star2").SetActive(false);
            GameObject.Find("Star3").SetActive(false);
            AudioManager.instance.Play("Victory1");
        }
        else if (TotalKills < 350)
        {
            GameObject.Find("Star3").SetActive(false);
            AudioManager.instance.Play("Victory2");
        }
        else
        {
            AudioManager.instance.Play("Victory3");
        }
        _statsDisplay[0].text = "Fire Rate: " + levels[0].ToString();
        _statsDisplay[1].text = "Speed: " + levels[1].ToString();
        _statsDisplay[2].text = "Damage: " + levels[2].ToString();
        _statsDisplay[3].text = "Spawn Rate: " + levels[3].ToString();
    }


    public void TimeOut()
    {
        StartCoroutine(TimeOutCR());
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

