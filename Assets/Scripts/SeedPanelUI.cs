using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedPanelUI : MonoBehaviour
{
    public List<GameObject> seeds = new List<GameObject>();

    public Color ready;
    public Color spent;

    int activeSeed = 0;

    private void Start()
    {
        //set all to ready 
        foreach (var _s in seeds) _s.GetComponent<Image>().color = ready;
    }

    public void UpdateSeed()
    {
        if (activeSeed > 8) return;
        activeSeed++;
        seeds[activeSeed - 1].GetComponent<Image>().color = spent;
        seeds[activeSeed - 1].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1) * 0.3f;

       
    }


    void Update()
    {
        
        
    }
}
