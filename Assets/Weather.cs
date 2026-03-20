using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weather : MonoBehaviour
{

    public Sprite rain, sun, thunder;


    public void Rain()
    {
        GetComponent<Image>().sprite = rain;
    }


    public void Sun()
    {
        GetComponent<Image>().sprite = sun;
    }

    public void Thunder()
    {
        GetComponent<Image>().sprite = thunder;
    }
}
