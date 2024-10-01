using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject ZombiePrefab;

    private float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            //Enable this code to get a feel for the range in which the zombie spawn
            //Instantiate(ZombiePrefab,transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            Instantiate(ZombiePrefab,transform);
            _timer = GameManager.stats[3];
        }
    }
}
