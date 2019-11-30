
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public Text moveText; // This line added by Daniel
    public Sprite isp;
    public Sprite wsp1;
    public Sprite wsp2;
    public Sprite wsp3;
    public Sprite wsp4;
    public Sprite wsp5;
    public Sprite ssp1;
    public Sprite ssp2;
    public Sprite ssp3;

    private int wspind = 0;
    private SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb2d;
    private bool swing;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !swing) { swing = true; /*turn on hurtbox*/ wspind = 0; }
    }

    /*Create method for Swing
     * Set a boolean which says we are swinging.
     * Don't move faster than a certain speed while swinging.
     * Hurtbox collider turns on during swing, turns off after.
     * Animations are controlled while swinging
     * 
     * Use a sprite atlas instead of public sprites?
     * Texture2D won't work either
     */

    void FixedUpdate()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        if (swing) // This will replace all of the rest of the method if we're swinging
        {
            if (wspind == 10) { wspind = 0; swing = false; }
            else if (wspind == 7) { spriteRenderer.sprite = ssp3; wspind++; }
            else if (wspind == 5) { spriteRenderer.sprite = ssp2; wspind++; }
            else if (wspind == 0) { spriteRenderer.sprite = ssp1; wspind++; }
            else { wspind++; };
            rb2d.velocity = move * speed * 0.15f;
            return;
        }
        moveText.text = "Horizontal: " + move.x.ToString() + " Vertical: " + move.y.ToString();
        rb2d.velocity = move*speed; // This is the line that moves our character
        //if (rb2d.velocity.y != 0.00f) spriteRenderer.sprite = jsp;
        if (move.magnitude != 0.0f)
        {
            if (wspind > 75/speed) { spriteRenderer.sprite = wsp5; wspind = 0; } // Rework this to not use greater than...
            else if (wspind > 60/speed) { spriteRenderer.sprite = wsp4; wspind++; }
            else if (wspind > 45/speed) { spriteRenderer.sprite = wsp3; wspind++; }
            else if (wspind > 30/speed) { spriteRenderer.sprite = wsp2; wspind++; }
            else if (wspind > 15/speed) { spriteRenderer.sprite = wsp1; wspind++; }
            else wspind++;
        }
        else spriteRenderer.sprite = isp;
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f)); // edited line to include -0.1f  to prevent flipping while stationary
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {

    }
    void OnTriggerExit2D(Collider2D other) // This block added by Daniel
    {

    }

}