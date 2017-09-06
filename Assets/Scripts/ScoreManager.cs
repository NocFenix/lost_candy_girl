using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public Text text;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        if (score < 0) score = 0;
        text.text = score.ToString();
    }

    /// <summary>
    /// Add points to the score
    /// </summary>
    /// <param name="points">Points to add to score</param>
    public static void AddPoints(int points)
    {
        score += points;
    }

    /// <summary>
    /// Resets all of the points
    /// </summary>
    public static void Reset()
    {
        score = 0;
    }
}
