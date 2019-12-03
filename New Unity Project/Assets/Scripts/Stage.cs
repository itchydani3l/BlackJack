using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour // This class controls the fade-in and fade-out of the screen shroud and transitioning between levels.
{
    public string nextLevel;
    public Image screen;
    public float duration;
    private bool loading;
    private bool inout;
    private bool leaving;
    void Start()
    {
        screen.enabled = true;
        screen.color = new Color(0f, 0f, 0f, 1f);
        loading = true;
        inout = false;
        leaving = false;
    }
    void Update()
    {
        if (loading) fadeImage(inout) ;
        else if (leaving) SceneManager.LoadScene(nextLevel);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (true) { leaving = true; loading = true; inout = true; } // Change this condition to include a level key?
        }
    }
    void fadeImage(bool fadein)
    {
        float alp = screen.color.a;
        if (alp > 0f && !fadein) alp -= 0.075f;
        else if (alp < 1f && fadein) alp += 0.075f;
        else loading = false;
        screen.color = new Color(0f,0f,0f, alp);
    }
}