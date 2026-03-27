using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{

    public Sprite earlyHit, perfectHit, lateHit;

 

    public void ShowIconAndMult(string _icon, int score = 0)
    {
        if(_icon == "early") GetComponent<Image>().sprite = earlyHit;
        else if(_icon == "perfect") GetComponent<Image>().sprite = perfectHit;
        else if (_icon == "perfect") GetComponent<Image>().sprite = lateHit;

        if (score > 1)
        {
            Color c = transform.Find("mult").GetComponent<Text>().color;
            
            float t = Mathf.Clamp01(score / 9f); // get values + clamp to 1
            t = Mathf.SmoothStep(0f, 1f, t); // steps 0 to 1
            c.a = Mathf.Lerp(0.02f, 0.5f, t); // choice of spread - controlled mapping

            transform.Find("mult").GetComponent<Text>().color = c;
            transform.Find("mult").GetComponent<Text>().text = "x" + (score);

        }

        // call mult udpate here too!
    }


}
