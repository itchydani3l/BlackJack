using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour // This class was written completely by Daniel
{
    public float wobble;
    public float initialVelocity;
    protected Rigidbody2D rb2d;
    private Vector2 vWobb;
    private float oPosition;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0f, initialVelocity);
        vWobb = new Vector2(0f, wobble);
        oPosition = gameObject.transform.position.y;
        //oPosition = 0f;
    }
    void Update()
    {
        if (rb2d.position.y > oPosition) rb2d.velocity -= vWobb;
        if (rb2d.position.y < oPosition) rb2d.velocity += vWobb;
    }
}