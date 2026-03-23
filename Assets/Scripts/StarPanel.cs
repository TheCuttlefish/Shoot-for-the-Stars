using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanel : MonoBehaviour
{
    private Animation ani;
    public StarScore starScore;
    private void Start()
    {
        ani = GetComponent<Animation>();
    }

    public void Shake()
    {
        ani.Play();
        starScore.UpdateStarScore();
    }
}
