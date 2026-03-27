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
            //transform.Find("mult").GetComponent<Text>().color = 
            transform.Find("mult").GetComponent<Text>().text = "x" + (score);

        }

        // call mult udpate here too!
    }


}
