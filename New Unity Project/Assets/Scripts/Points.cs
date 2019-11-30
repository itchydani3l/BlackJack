/*
     hmwk4_BusbyDaniel.cs
    
     Daniel Busby
     Account: dbusby1
     CSc 4821
     Homework 4
     Due date: 11/19
    
     Description:
         This is platformer game for Homework 4
     Input:
         WASD
     Output:
         uh, the screen

     Lines made by Daniel are indicated as such, all other were made by the tutorial author listed in the Homework 3 Assignment.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Points : MonoBehaviour
{
    private int count;
    public int requiredPoints; // This line added by Daniel
    public Text countText; // This line added by Daniel
    public Text winText; // This line added by Daniel

    private bool key; // This line added by Daniel
    public Image uikey; // This line added by Daniel

    public string nextLevel; // This line added by Daniel

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        CountText();
        key = false; // This line added by Daniel
    } 

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item") 
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            CountText();
        }
        if (other.gameObject.tag == "Key") // This block was added by Daniel
        {
            key = true;
            uikey.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Lock" && key) // This block was added by Daniel
        {
            key = false;
            uikey.gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Goal") // This block was added by Daniel
        {
            if (count >= requiredPoints) { winText.text = "YOU WIN!"; /*gameObject.SetActive(false);*/ } // This is commented out temporarily because the camera is nested under the player sprite
                winText.gameObject.SetActive(true);
        }
        if (other.gameObject.tag == "Door") // This block was added by Daniel
        {
            if (count >= requiredPoints) { SceneManager.LoadScene(nextLevel);}
            winText.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other) // This block added by Daniel
    {
        if (other.gameObject.tag == "Goal" && count<requiredPoints) winText.gameObject.SetActive(false);
        if (other.gameObject.tag == "Door" && count < requiredPoints) winText.gameObject.SetActive(false);
    }
    void CountText()
    {
        countText.text = "Points: " + count.ToString();
    }
}