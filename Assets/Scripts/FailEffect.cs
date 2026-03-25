using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailEffect : MonoBehaviour
{
    public RectTransform uiTarget;
    ParticleSystem particles;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        
    }

    public void Play()
    {
        
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiTarget.TransformPoint(uiTarget.rect.center));
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos) + new Vector3(0, 0, 0);
        worldPos.z = 0f;
        transform.position = worldPos;
        particles.Play();
    }


  

}