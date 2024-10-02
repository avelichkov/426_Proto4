using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private GameObject player;
    public float speed;
    // Start is called before the first frame update
    void Awake()
    {
        transform.position = GetPosition();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private Vector2 GetPosition()
    {
        float angle = Random.Range(0f, 2 * Mathf.PI);
        float radius = 14.0f;
        float xPos = Mathf.Cos(angle) * radius;
        float yPos = Mathf.Sin(angle) * radius;
        return new Vector2(xPos, yPos);
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 dir = player.transform.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.position = Vector2.MoveTowards(this.transform.position,
        player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
