using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    public Animation start, timer, wave;


    public void StartGame()
    {

        start.Play("hideStart");
        timer.Play("resetWin");
        wave.Play("waveFast");
    }

    public void StopGame()
    {
        start.Play("showStart");
        wave.Stop();
    }

    public void Win()
    {
        timer.Play("win");
    }

}
