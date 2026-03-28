using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    Animation start;

    int hintCounter = 0;
    // start with 3 hints
    // --- count down to 0 if fail to show hint again
    // --- a win resets hints to 3 ( you need to fail 3 times)

    private void Start()
    {
        start = GetComponent<Animation>();
    }

    public void StartGame()
    {
        start.Play("hideStart");
    }

    public void Show()
    {
        
        start.Play("showStart");

        hintCounter--;
        if (hintCounter < 0)
        start.transform.Find("stop here").gameObject.SetActive(true);
        else start.transform.Find("stop here").gameObject.SetActive(false);
    }

    public void Hide()
    {
        hintCounter = 3;
        // hide this after first game!!!!
        start.transform.Find("stop here").gameObject.SetActive(false);
    }

}
