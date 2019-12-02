
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float speed; // the speed of the character
    public int amode; // assigned mode
    public int adir; // assigned direction in unity
    private int odir; // original direction

    public int[] timing; // timing counts for guard patrol
    private int tind; // timing index

    public Sprite[] north; // These are all of our sprite arrays.
    public Sprite[] south;
    public Sprite[] side;
    private Sprite[] current;

    public GameObject cone;
    public GameObject projectile;
    public float projectileSpeed;

    private Vector3[] compass = { 
        new Vector3(1f, 0f, 0f), 
        new Vector3(0f, 1f, 0f), 
        new Vector3(-1f, 0f, 0f), 
        new Vector3(0f, -1f, 0f),
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 0f, 90f),
        new Vector3(0f, 0f, 180f),
        new Vector3(0f, 0f, 270f)}; // Don't change this
    private Vector3 direction; // our current direction
    

    private int mode; // current mode
    private int wspind = 0; // Walk sprite index, AKA animation counter
    private SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb2d;
    protected BoxCollider2D box2;

    private Collider2D targetColl;
    private Vector3 target; // our last sighted target position
    private bool swing;
    private bool targetDetected; // Whether we are currently engaged in combat

    // Mode 1 = standing, mode 2 = clockwise, mode 3 = CCW, mode 4 = sighted target, mode 5 = unconscious, mode 6 = dying, mode 7 = recovering

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        box2 = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swing = false;
        targetDetected = false;
        mode = amode;
        odir = adir;
        direction = Vector3.zero;
        if (timing.Length == 1) timing = new int[8] { 0, timing[0], 0, timing[0], 0, timing[0], 0, timing[0]};
        else if (timing.Length == 2) timing = new int[8] { 0, timing[0], 0, timing[1], 0, timing[0], 0, timing[1] };
        else if (timing.Length == 3) timing = new int[8] { timing[2], timing[0], timing[2], timing[1], timing[2], timing[0], timing[2], timing[1] };
        else if (timing.Length == 6) timing = new int[8] { 1, 0, 1, 0, 1, 0, 1, 0 };
        else if (timing.Length == 7) timing = new int[8] { 0,1,0,1,0,1,0,1 };
        for (int i = 1; i < timing.Length; i++) timing[i] += timing[i - 1]; // This is important. This converts our relative timings to absolute timings
    }

    //void enemyInputs() { }// This block will detect when to trigger our abilities

    void enemyMovement(float x, float y) // I break it down in this method to prevent the update methods from getting cluttered
    {
        Vector2 move = Vector2.zero;
        move.x = x;
        move.y = y;
        rb2d.velocity = move * speed; //=============================================== This is the line that moves our character
        if (move.magnitude != 0.0f)
        {
            // Play our walking animation
            if (move.x * move.x > move.y * move.y)
            {
                current = side;
            }
            else
            {
                if (move.y > 0.0f)
                {
                    current = north;
                }
                else
                {
                    current = south;
                }
            }
            if (wspind > 30 / speed) { spriteRenderer.sprite = current[1]; wspind = 0; } // This is our horizontal (x) sprites
            else if (wspind > 15 / speed) { spriteRenderer.sprite = current[0]; wspind++; }
            else wspind++;
        }
        else
        {
            //Play our idle animation
            if (adir == 0 || adir==2)
            {
                current = side;
            }
            if (adir == 1)
            {
                current = north;
            }
            if (adir == 3)
            {
                current = south;
            }
            if (wspind > 70 / speed) { spriteRenderer.sprite = current[3]; wspind = 0; } // Rework this to not use greater than...
            else if (wspind > 35 / speed) { spriteRenderer.sprite = current[2]; wspind++; }
            else wspind++;
        }
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f)); // edited line to include -0.1f  to prevent flipping while stationary
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (mode < 5) enemyInputs(); // If we're not unconscious, then we'll calculate our attacks
    }

    void FixedUpdate()
    {
        if (swing) doSwing();
        //else if (targetDetected) tryDetect(targetColl.transform.position); // If we're in combat right now, then continue to engage
        else if (mode == 1) mode1();
        else if (mode == 2) mode2();
        else if (mode == 3) mode3();
        else if (mode == 4) mode4();
        else if (mode == 5) mode5();
    }
    void turnDirection(int dir) // this turns our wizard, and it also turns our detection cone
    {
        adir = dir;
        cone.transform.eulerAngles = compass[dir + 4];
    }
    void faceTarget() // We want to face our target during combat
    {
        Vector3 faceAngle = target - transform.position;
        if (faceAngle.x * faceAngle.x > faceAngle.y * faceAngle.y)
        {
            if (faceAngle.x > 0) { turnDirection(0); spriteRenderer.flipX = false; }
            else { turnDirection(2); spriteRenderer.flipX = true; }
                current = side;
        }
        else
        {
                if (faceAngle.y > 0.0f)
                {
                current = north;
                turnDirection(1);
                }
                else
                {
                current = south;
                turnDirection(3);
                }
        }
    }
    void doSwing() // This plays our attack animation, and calls the shoot function at the end of it
    {
            if (wspind == 15) { wspind = 0; swing = false; shoot(); }
            else if (wspind == 10) { spriteRenderer.sprite = current[4]; wspind++; }
            else if (wspind == 5) { spriteRenderer.sprite = current[5]; wspind++; }
            else if (wspind == 0) { spriteRenderer.sprite = current[6]; wspind++; }
            else { wspind++; };
    }
    void shoot()
    {
        //projectile set position
        //projectile set rotation (from current rotation)
        //projectile turn on
        //projectile set velocity
        if (projectile.activeSelf) return;
        Vector3 myPosition = transform.position;
        Vector3 deltaPos = target - myPosition;
        projectile.transform.position = myPosition + deltaPos.normalized*0.5f;
        projectile.transform.eulerAngles = compass[adir + 4];
        projectile.SetActive(true);
        projectile.GetComponent<Rigidbody2D>().velocity = deltaPos.normalized * projectileSpeed;
    }
    void mode1()
    {
        enemyMovement(0f, 0f);
    }
    void mode2() // Move around clockwise in a rectangle
    {
        // (Turn) adir = 3
        // (Walk) direction = compass[adir]
        // (Wait) direction = Vector2.zero
        if (tind == timing[7] * 15 + timing[0]*15 - 1) { turnDirection(odir); tind = timing[0] * 15; }                // Turn
        else if (tind == timing[7] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[6] * 15) { direction = compass[(3 + odir) % 4]; tind++; }    // Move


        else if (tind == timing[6] * 15 - 1) { turnDirection((3 + odir) % 4); tind++; }                // Turn
        else if (tind == timing[5] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[4] * 15) { direction = compass[(2 + odir) % 4]; tind++; }    // Move


        else if (tind == timing[4] * 15 - 1) { turnDirection((2 + odir) % 4); tind++; }              // Turn
        else if (tind == timing[3] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[2] * 15) { direction = compass[(1 + odir) % 4]; tind++; }    // Move


        else if (tind == timing[2] * 15-1) { turnDirection((1 + odir) % 4); tind++; }                // Turn 
        else if (tind == timing[1] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[0] * 15) { direction = compass[odir]; tind++; }    // Move

        else { tind++; };
        enemyMovement(direction.x, direction.y);
    }
    void mode3()  // Move around counter clockwise in a rectangle
    {
        if (tind == timing[7] * 15 + timing[0] * 15 - 1) { turnDirection(odir); tind = timing[0] * 15; }                // Turn
        else if (tind == timing[7] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[6] * 15) { direction = compass[(1 + odir) % 4]; tind++; }    // Move


        else if (tind == timing[6] * 15 - 1) { turnDirection((1 + odir) % 4); tind++; }                // Turn
        else if (tind == timing[5] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[4] * 15) { direction = compass[(2 + odir) % 4]; tind++; }    // Move


        else if (tind == timing[4] * 15 - 1) { turnDirection((2 + odir) % 4); tind++; }              // Turn
        else if (tind == timing[3] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[2] * 15) { direction = compass[(3 + odir) % 4]; tind++; }    // Move


        else if (tind == timing[2] * 15 - 1) { turnDirection((3 + odir) % 4); tind++; }                // Turn 
        else if (tind == timing[1] * 15) { direction = Vector3.zero; tind++; }  // Wait
        else if (tind == timing[0] * 15) { direction = compass[odir]; tind++; }    // Move

        else { tind++; };
        enemyMovement(direction.x, direction.y);
    }
    void mode4()
    {
        enemyMovement(0f, 0f);
    }
    void mode5() // Mode 5 = Recovering from unconsciousness
    {
            if (wspind == 200) { mode = amode; wspind = 0; box2.enabled = true; } 
            else if (wspind % 100 == 0) { spriteRenderer.sprite = south[8]; wspind++; }
            else if (wspind % 50 == 0) { spriteRenderer.sprite = south[7]; wspind++; }
            else wspind++;
    }
    void tryDetect(Vector3 test)
    {
        if (mode > 4) return;                                   // If we're unconscious, we don't detect the player
        Vector3 myPosition = transform.position;
        float distance = 6f;                                    // We can replace this with a public line of sight float
        Debug.DrawRay(myPosition, test - myPosition, Color.red, 1000f, false);
        RaycastHit2D sighttest = Physics2D.Raycast(myPosition, test - myPosition, distance);
        if (!sighttest) { targetDetected = false; return; } // We need to check if we actually saw anything, to avoid null pointer errors.
        if (sighttest.collider.tag == "Player") // Player has been detected
        {
            targetDetected = true;
            targetColl = sighttest.collider; // We lock onto the target;
            target = test;
            faceTarget();
            if (!swing) // If we aren't swinging, then we can swing at the enemy
            {
                swing = true;
                wspind = 0;
                rb2d.velocity = Vector2.zero;
            }
        }
        else targetDetected = false;
    }
    public void detectPlayer(Collider2D other)
    {
        target = other.transform.position;
        tryDetect(target);
    }
    void OnTriggerEnter2D(Collider2D other) // This may need fixing, for reliability
    {
        if (other.gameObject.tag == "Hurt" && mode!=5) // this is how we get hurt. Ouch!
        {
            mode = 5;
            wspind = 0;
            box2.enabled = false; // When we are unconscious, we don't block movement
            rb2d.velocity = Vector2.zero;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
}