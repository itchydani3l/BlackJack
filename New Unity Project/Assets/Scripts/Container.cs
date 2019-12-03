using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour // This class was written completely by Daniel
{
    protected bool opened;
    public GameObject contents;
    public Sprite osprite;

    void OnEnable()
    {
        contents.SetActive(false);
        opened = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !opened)
        {
            contents.SetActive(true);
            //contents.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 2f); // This was intended to make the objects rise a bit, but it wasn't playing nice with the other scripts
            opened = true;
            GetComponent<SpriteRenderer>().sprite = osprite;
        }
    }
}