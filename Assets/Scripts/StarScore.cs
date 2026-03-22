using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarScore : MonoBehaviour
{
    int score = 0;
    Text scoreText;
    // Start is called before the first frame update
    void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    public void UpdateStarScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
