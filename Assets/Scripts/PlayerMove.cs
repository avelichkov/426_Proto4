using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 5f; // Speed of the player movement
    private float startSpeed;
    public Rigidbody2D rb; // Reference to the Rigidbody2D (for 2D games)
    private float currentSpeed;

    private SpriteRenderer sr;
    private SpriteRenderer srGun;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = GameManager.stats[1];
        sr = GetComponent<SpriteRenderer>();
        srGun = GameObject.FindWithTag("Gun").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        currentSpeed = GameManager.stats[1];
        // Get input from WASD keys
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float moveY = Input.GetAxis("Vertical");   // W/S or Up/Down arrows

        // Create movement vector
        Vector2 movement = new Vector2(moveX, moveY);

        // Apply movement to the Rigidbody2D
        //Debug.Log(currentSpeed);
        rb.velocity = movement * currentSpeed;
    }

    public void UpdateColor(float percent)
    {
        Debug.Log(percent);
        sr.color = Color.Lerp(Color.white,Color.green,percent);
        srGun.color = sr.color;
    }
}
