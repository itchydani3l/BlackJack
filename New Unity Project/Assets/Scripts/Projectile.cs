using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour // This class was written completely by Daniel
{
    public Sprite[] sprites;
    private int wspind = 0;
    private bool exploding;
    private SpriteRenderer spriteRenderer;
    //protected BoxCollider2D[] boxes;
    protected BoxCollider2D[] boxes;
    protected Rigidbody2D rb2d;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxes = GetComponents<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        boxes[0].enabled = true;
        boxes[1].enabled = false;

    }
    void OnEnable()
    {
        boxes[0].enabled = true;
        boxes[1].enabled = false;
        wspind = 0;
        exploding = false;
    }
    void FixedUpdate() // For the projectile, we just need to animate the sprites, that's all. All of the velocity is handled by the parent
    {
        if (exploding) explode();
        else fly();
    }
    void fly() // This plays the flying animation indefinitely
    {
        if (wspind == 20) { wspind = 0; spriteRenderer.sprite = sprites[4]; }
        else if (wspind == 16) { spriteRenderer.sprite = sprites[3]; wspind++; }
        else if (wspind == 12) { spriteRenderer.sprite = sprites[2]; wspind++; }
        else if (wspind == 8) { spriteRenderer.sprite = sprites[1]; wspind++; }
        else if (wspind == 4) { spriteRenderer.sprite = sprites[0]; wspind++; }
        else wspind++;
    }
    void explode() // This plays the explode animation and turns off the object, so it's ready to be launched again
    {
        if (wspind == 18) { wspind = 0; spriteRenderer.sprite = sprites[0]; gameObject.SetActive(false); }
        else if (wspind == 15) { spriteRenderer.sprite = sprites[10]; wspind++; }
        else if (wspind == 12) { spriteRenderer.sprite = sprites[9]; wspind++; } 
        else if (wspind == 9) { spriteRenderer.sprite = sprites[8]; wspind++; }
        else if (wspind == 6) { spriteRenderer.sprite = sprites[7]; wspind++; boxes[1].enabled = false; }// Our explosion stops hurting people after the first 2 frames.
        else if (wspind == 3) { spriteRenderer.sprite = sprites[6]; wspind++; }
        else if (wspind == 0) { spriteRenderer.sprite = sprites[5]; wspind++; }
        else wspind++;
    }
    void OnCollisionEnter2D(Collision2D other) // When we detect a collision, we will stop in place, turn on our hurtboxes, and start playing the explode animation.
    {
        exploding = true;
        wspind = 0;
        boxes[1].enabled = true;
        boxes[0].enabled = false;
        rb2d.velocity = Vector3.zero;
    }
}