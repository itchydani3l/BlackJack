
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private bool key;
    public Image uikey;

    //public string nextLevel; // This has been moved to stage controller

    public Sprite isp;
    public Sprite wsp1;
    public Sprite wsp2;
    public Sprite wsp3;
    public Sprite wsp4;
    public Sprite wsp5;
    public Sprite ssp1;
    public Sprite ssp2;
    public Sprite ssp3;

    public GameObject hurtbox;

    private int count;
    private int wspind = 0;
    private SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb2d;

    private bool swing;

    public GameObject pauseCanvas;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void pauseGame()
    {
        if (!paused) { paused = true; Time.timeScale = 0; pauseCanvas.SetActive(true); }
        else { paused = false; Time.timeScale = 1; pauseCanvas.SetActive(false); }
    }
    void playerInputs()
    {
        if (Input.GetButtonDown("Cancel")) pauseGame();
        if (Input.GetButtonDown("Jump") && !swing) { swing = true; hurtbox.SetActive(true); wspind = 0; }
    }
    void playerMovement() // I break it down in this method to prevent the update methods from getting cluttered
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        if (swing) // This will replace all of the rest of the method if we're swinging
        {
            if (wspind == 10) { wspind = 0; swing = false; hurtbox.SetActive(false);}
            else if (wspind == 7) { spriteRenderer.sprite = ssp3; wspind++; }
            else if (wspind == 5) { spriteRenderer.sprite = ssp2; wspind++; }
            else if (wspind == 0) { spriteRenderer.sprite = ssp1; wspind++; }
            else { wspind++; };
            rb2d.velocity = move * speed * 0.15f;
            return;
        }
        //moveText.text = "Horizontal: " + move.x.ToString() + " Vertical: " + move.y.ToString();
        rb2d.velocity = move * speed; // =======================================================This is the line that moves our character
        //if (rb2d.velocity.y != 0.00f) spriteRenderer.sprite = jsp;
        if (move.magnitude != 0.0f)
        {
            if (wspind > 75 / speed) { spriteRenderer.sprite = wsp5; wspind = 0; } // Rework this to not use greater than...
            else if (wspind > 60 / speed) { spriteRenderer.sprite = wsp4; wspind++; }
            else if (wspind > 45 / speed) { spriteRenderer.sprite = wsp3; wspind++; }
            else if (wspind > 30 / speed) { spriteRenderer.sprite = wsp2; wspind++; }
            else if (wspind > 15 / speed) { spriteRenderer.sprite = wsp1; wspind++; }
            else wspind++;
        }
        else spriteRenderer.sprite = isp;
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f)); // edited line to include -0.1f  to prevent flipping while stationary
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            hurtbox.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
            hurtbox.GetComponent<Transform>().position += new Vector3((spriteRenderer.flipX ? -1.2f : 1.2f),0f,0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerInputs();
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
        playerMovement();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            other.gameObject.SetActive(false);
            count = count + 100;
            CountText();
        }
        if (other.gameObject.tag == "Hurt") // If we get hurt, we die!
        {
            lose();
        }
        if (other.gameObject.tag == "Key" && !key)
        {
            key = true;
            uikey.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Lock" && key)
        {
            key = false;
            uikey.gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Goal")
        {
            if (true) ; // Change this condition
            winText.gameObject.SetActive(true);
        }
        if (other.gameObject.tag == "Door")
        {
            //if (true) { SceneManager.LoadScene(nextLevel); } // This has been moved to stage controller
        }
    }
    void lose()
    {
        winText.text = "YOU DIED!";
        winText.gameObject.SetActive(true);
        pauseGame();
        Time.timeScale = .5f;
        gameObject.SetActive(false);
    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
    void CountText()
    {
        countText.text = "$" + count.ToString();
    }
}