using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    public Animation start, timer;

    int hintCounter = 0;
    // start with 3 hints
    // --- count down to 0 if fail to show hint again
    // --- a win resets hints to 3 ( you need to fail 3 times)

    public void StartGame()
    {

        start.Play("hideStart");
        timer.Play("resetWin");
        
    }

    public void StopGame()
    {
        
        start.Play("showStart");

        hintCounter--;
        if (hintCounter < 0)
        start.transform.Find("stop here").gameObject.SetActive(true);
        else start.transform.Find("stop here").gameObject.SetActive(false);
    }

    public void Win()
    {
        hintCounter = 3;
        timer.Play("win");

        // hide this after first game!!!!
        start.transform.Find("stop here").gameObject.SetActive(false);
    }

}
