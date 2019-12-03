using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour // This class keeps the score across levels
{
    public static Score GM;
    public int score;

    void Awake()
    {
        if (GM != null)
            GameObject.Destroy(GM);
        else
            GM = this;

        DontDestroyOnLoad(this);
    }
}