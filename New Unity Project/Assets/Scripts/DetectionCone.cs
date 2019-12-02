using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCone : MonoBehaviour // This class was written completely by Daniel
{
    private string dTag;
    private string parentScript;
    void Start()
    {
        dTag = "Player";
        parentScript = "Enemy";
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == dTag)
        {
            gameObject.GetComponentInParent<Enemy>().detectPlayer(other);
        }
    }
}