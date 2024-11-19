using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private GameObject player;
    public float speed;
    private Collider2D zombieCol;

    // Start is called before the first frame update
    void Awake()
    {
        transform.position = GetPosition();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private Vector2 GetPosition()
    {
        Vector2 playerPos = GameObject.Find("Player").transform.position;
        float angle = Random.Range(0f, 2 * Mathf.PI);
        float radius = 15.0f;
        float xPos = playerPos.x + Mathf.Cos(angle) * radius;
        float yPos = playerPos.y + Mathf.Sin(angle) * radius;
        return new Vector2(xPos, yPos);
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 dir = player.transform.position;
        dir.Normalize();
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.position = Vector2.MoveTowards(this.transform.position,
        player.transform.position, speed * Time.deltaTime);

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GameManager.instance.ZombieKilled();
            BulletShot bullet = other.gameObject.GetComponent<BulletShot>();
            bullet.TakeDamage();
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Colliding");
            //GameManager.instance.GameOver();
        }
    }

}
