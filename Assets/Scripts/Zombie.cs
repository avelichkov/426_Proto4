using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.position = GetPosition();
    }
    private Vector2 GetPosition()
    {
        float angle = Random.Range(0f,2*Mathf.PI);
        float radius = 14.0f;
        float xPos = Mathf.Cos(angle) * radius;
        float yPos = Mathf.Sin(angle) * radius;
        return new Vector2(xPos,yPos);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
