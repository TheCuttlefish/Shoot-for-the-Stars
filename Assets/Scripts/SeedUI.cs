using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedUI : MonoBehaviour
{

    Animation ani;
    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponent<Animation>();
        ani.Stop();
        ani.clip.SampleAnimation(gameObject, 0);
    }

    // Update is called once per frame
    public void StopSpin()
    {
        GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
        GetComponent<RectTransform>().localScale = new Vector3(0.6991677f, 0.6991677f, 0f);
        ani.Stop();
        ani.clip.SampleAnimation(gameObject, 0);
        
    }

    public void StartSpin()
    {
        ani.Play();
    }

}
