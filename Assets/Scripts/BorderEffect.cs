using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderEffect : MonoBehaviour
{

    Animator effect;
    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void PlayEffect()
    {
        effect.enabled = true;
        effect.Play("border_ani",0,0f);
    }
}
